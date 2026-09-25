using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using Point = System.Windows.Point;

namespace FolderThemeStudio.App.Services;

public sealed record ExplorerHoverTarget(string FolderPath, Rect ItemBounds, nint RootWindowHandle = default);

public enum HoverSurface { ExplorerList, DesktopList, Other }

public sealed record HoverListItemCandidate(
    string Name,
    int ProcessId,
    nint RootWindowHandle,
    Rect Bounds,
    string ProcessName,
    bool IsFileListItem,
    HoverSurface Surface = HoverSurface.ExplorerList);

public interface IHoverListItemProbe
{
    HoverListItemCandidate? FindListItem(Point screenPoint);
}

public interface IExplorerWindowPathResolver
{
    string? GetCurrentFolderPath(nint rootWindowHandle);
}

public sealed class ExplorerFolderHoverTargetResolver : IExplorerFolderHoverTargetResolver
{
    private readonly IHoverListItemProbe itemProbe;
    private readonly IExplorerWindowPathResolver windowPathResolver;
    private readonly Func<string, bool> directoryExists;
    private readonly DesktopFolderPathResolver desktopPathResolver;

    public ExplorerFolderHoverTargetResolver(
        IHoverListItemProbe? itemProbe = null,
        IExplorerWindowPathResolver? windowPathResolver = null,
        Func<string, bool>? directoryExists = null,
        DesktopFolderPathResolver? desktopPathResolver = null)
    {
        this.itemProbe = itemProbe ?? new WindowsHoverListItemProbe();
        this.windowPathResolver = windowPathResolver ?? new ShellExplorerWindowPathResolver();
        this.directoryExists = directoryExists ?? Directory.Exists;
        this.desktopPathResolver = desktopPathResolver ?? new DesktopFolderPathResolver();
    }

    public ExplorerHoverTarget? TryResolve(Point screenPoint)
    {
        if (!double.IsFinite(screenPoint.X) || !double.IsFinite(screenPoint.Y)) return null;
        try
        {
            var item = itemProbe.FindListItem(screenPoint);
            if (item is null || !item.IsFileListItem ||
                !string.Equals(item.ProcessName, "explorer", StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrWhiteSpace(item.Name) || item.Bounds.IsEmpty || !item.Bounds.Contains(screenPoint))
                return null;

            if (item.Surface == HoverSurface.DesktopList)
            {
                var desktopPath = desktopPathResolver.Resolve(item.Name);
                return desktopPath is null ? null : new ExplorerHoverTarget(desktopPath, item.Bounds, item.RootWindowHandle);
            }
            if (item.Surface != HoverSurface.ExplorerList) return null;
            var parentPath = windowPathResolver.GetCurrentFolderPath(item.RootWindowHandle);
            if (string.IsNullOrWhiteSpace(parentPath) || !Path.IsPathFullyQualified(parentPath)) return null;
            var leafName = item.Name.Trim();
            if (leafName is "." or ".." || Path.IsPathRooted(leafName) ||
                !string.Equals(Path.GetFileName(leafName), leafName, StringComparison.Ordinal))
                return null;

            var folderPath = Path.TrimEndingDirectorySeparator(Path.GetFullPath(Path.Combine(parentPath, leafName)));
            return directoryExists(folderPath) ? new ExplorerHoverTarget(folderPath, item.Bounds, item.RootWindowHandle) : null;
        }
        catch (Exception exception) when (exception is ArgumentException or IOException or UnauthorizedAccessException
            or InvalidOperationException or COMException or ElementNotAvailableException)
        {
            return null;
        }
    }
}

internal sealed class WindowsHoverListItemProbe : IHoverListItemProbe
{
    public HoverListItemCandidate? FindListItem(Point screenPoint)
    {
        var element = AutomationElement.FromPoint(screenPoint);
        var walker = TreeWalker.ControlViewWalker;
        for (var depth = 0; element is not null && depth < 8; depth++)
        {
            var current = element.Current;
            if (current.ControlType == ControlType.ListItem || current.ControlType == ControlType.DataItem)
            {
                if (current.BoundingRectangle.IsEmpty) return null;
                var isFileListItem = IsWithinListControl(element, walker);
                var hwnd = NativeMethods.GetAncestor(NativeMethods.WindowFromPoint(
                    new NativeMethods.NativePoint((int)screenPoint.X, (int)screenPoint.Y)), NativeMethods.GetAncestorRoot);
                if (hwnd == nint.Zero) return null;
                var rootClass = GetWindowClass(hwnd);
                var surface = rootClass is "Progman" or "WorkerW" ? HoverSurface.DesktopList : HoverSurface.ExplorerList;
                string processName;
                try
                {
                    using var process = Process.GetProcessById(current.ProcessId);
                    processName = process.ProcessName;
                }
                catch (Exception exception) when (exception is ArgumentException or System.ComponentModel.Win32Exception)
                {
                    return null;
                }
                return new HoverListItemCandidate(
                    current.Name,
                    current.ProcessId,
                    hwnd,
                    current.BoundingRectangle,
                    processName,
                    isFileListItem,
                    surface);
            }
            element = walker.GetParent(element);
        }
        return null;
    }

    private static bool IsWithinListControl(AutomationElement item, TreeWalker walker)
    {
        var parent = walker.GetParent(item);
        for (var depth = 0; parent is not null && depth < 6; depth++)
        {
            var type = parent.Current.ControlType;
            if (type == ControlType.List) return true;
            if (type == ControlType.Tree || type == ControlType.TreeItem || type == ControlType.Pane) return false;
            parent = walker.GetParent(parent);
        }
        return false;
    }

    private static string GetWindowClass(nint hwnd)
    {
        var result = new StringBuilder(256);
        return NativeMethods.GetClassName(hwnd, result, result.Capacity) > 0 ? result.ToString() : string.Empty;
    }

    private static class NativeMethods
    {
        internal const uint GetAncestorRoot = 2;

        [StructLayout(LayoutKind.Sequential)]
        internal readonly struct NativePoint(int x, int y)
        {
            internal readonly int X = x;
            internal readonly int Y = y;
        }

        [DllImport("user32.dll")]
        internal static extern nint WindowFromPoint(NativePoint point);

        [DllImport("user32.dll")]
        internal static extern nint GetAncestor(nint hwnd, uint flags);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern int GetClassName(nint hwnd, StringBuilder className, int maxCount);
    }
}

public sealed class DesktopFolderPathResolver
{
    private readonly string userDesktop;
    private readonly string publicDesktop;
    private readonly Func<string, bool> directoryExists;

    public DesktopFolderPathResolver(string? userDesktop = null, string? publicDesktop = null, Func<string, bool>? directoryExists = null)
    {
        this.userDesktop = userDesktop ?? Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        this.publicDesktop = publicDesktop ?? Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
        this.directoryExists = directoryExists ?? Directory.Exists;
    }

    public string? Resolve(string itemName)
    {
        if (string.IsNullOrWhiteSpace(itemName)) return null;
        var name = itemName.Trim();
        if (name is "." or ".." || Path.IsPathRooted(name) || !string.Equals(Path.GetFileName(name), name, StringComparison.Ordinal))
            return null;
        var matches = new[] { userDesktop, publicDesktop }
            .Where(root => !string.IsNullOrWhiteSpace(root))
            .Select(root => Path.GetFullPath(Path.Combine(root, name)))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Where(directoryExists)
            .Take(2)
            .ToArray();
        return matches.Length == 1 ? matches[0] : null;
    }
}

internal sealed class ShellExplorerWindowPathResolver : IExplorerWindowPathResolver
{
    private const BindingFlags InvokeFlags = BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance;

    public string? GetCurrentFolderPath(nint rootWindowHandle)
    {
        var shellType = Type.GetTypeFromProgID("Shell.Application");
        if (shellType is null) return null;
        var shell = Activator.CreateInstance(shellType);
        if (shell is null) return null;
        object? windows = null;
        try
        {
            windows = Invoke(shell, "Windows");
            if (windows is null) return null;
            var count = Convert.ToInt32(Read(windows, "Count"), System.Globalization.CultureInfo.InvariantCulture);
            for (var index = 0; index < count; index++)
            {
                object? browser = null;
                object? document = null;
                object? folder = null;
                object? self = null;
                try
                {
                    browser = Invoke(windows, "Item", index);
                    if (browser is null || Convert.ToInt64(Read(browser, "HWND"), System.Globalization.CultureInfo.InvariantCulture) != rootWindowHandle.ToInt64())
                        continue;
                    document = Read(browser, "Document");
                    if (document is null) return null;
                    folder = Read(document, "Folder");
                    if (folder is null) return null;
                    self = Read(folder, "Self");
                    return Convert.ToString(Read(self, "Path"), System.Globalization.CultureInfo.InvariantCulture);
                }
                finally
                {
                    Release(self);
                    Release(folder);
                    Release(document);
                    Release(browser);
                }
            }
            return null;
        }
        finally
        {
            Release(windows);
            Release(shell);
        }
    }

    private static object? Invoke(object target, string member, params object[] arguments) =>
        target.GetType().InvokeMember(member, InvokeFlags, null, target, arguments);

    private static object? Read(object? target, string member) => target is null ? null : Invoke(target, member);

    private static void Release(object? value)
    {
        if (value is not null && Marshal.IsComObject(value)) Marshal.FinalReleaseComObject(value);
    }
}
