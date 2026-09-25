using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Threading;
using Point = System.Windows.Point;

namespace FolderThemeStudio.App.Services;

public sealed class FolderQuickEditMouseHook : IDisposable
{
    private const int WhMouseLl = 14;
    private const int WmRightDown = 0x0204;
    private const int WmRightUp = 0x0205;
    private const int WmMiddleDown = 0x0207;
    private const int WmMiddleUp = 0x0208;
    private const int GaRoot = 2;
    private readonly FolderHoverMonitor monitor;
    private readonly Action<string> openEditor;
    private readonly Dispatcher dispatcher;
    private readonly FolderQuickEditDecision decision;
    private readonly HookProc callback;
    private nint handle;

    public FolderQuickEditMouseHook(FolderHoverMonitor monitor, Action<string> openEditor, FolderEditChord chord, Dispatcher dispatcher)
    {
        this.monitor = monitor;
        this.openEditor = openEditor;
        this.dispatcher = dispatcher;
        decision = new FolderQuickEditDecision(chord);
        callback = OnMouseEvent;
        handle = SetWindowsHookEx(WhMouseLl, callback, GetModuleHandle(null), 0);
        if (handle == nint.Zero) throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to install folder quick-edit mouse hook.");
    }

    public void UpdateChord(FolderEditChord chord) => decision.UpdateChord(chord);

    public void Dispose()
    {
        if (handle == nint.Zero) return;
        UnhookWindowsHookEx(handle);
        handle = nint.Zero;
    }

    private nint OnMouseEvent(int code, nint message, nint data)
    {
        if (code < 0 || handle == nint.Zero) return CallNextHookEx(handle, code, message, data);
        var kind = message.ToInt32();
        var button = kind is WmRightDown or WmRightUp ? FolderMouseButton.Right : FolderMouseButton.Middle;
        if (kind is WmRightUp or WmMiddleUp)
            return decision.OnMouseUp(button) ? new nint(1) : CallNextHookEx(handle, code, message, data);
        if (kind is not (WmRightDown or WmMiddleDown)) return CallNextHookEx(handle, code, message, data);

        var mouse = Marshal.PtrToStructure<MouseHookData>(data);
        var point = new Point(mouse.Position.X, mouse.Position.Y);
        var root = GetAncestor(WindowFromPoint(mouse.Position), GaRoot);
        var target = monitor.TryGetRecentTarget(point, root);
        var path = decision.OnMouseDown(button, CurrentModifiers(), point, target);
        if (path is null) return CallNextHookEx(handle, code, message, data);
        _ = dispatcher.BeginInvoke(() => openEditor(path));
        return new nint(1);
    }

    private static ModifierKeys CurrentModifiers()
    {
        var result = ModifierKeys.None;
        if (IsDown(0x12)) result |= ModifierKeys.Alt;
        if (IsDown(0x11)) result |= ModifierKeys.Control;
        if (IsDown(0x10)) result |= ModifierKeys.Shift;
        if (IsDown(0x5B) || IsDown(0x5C)) result |= ModifierKeys.Windows;
        return result;
    }

    private static bool IsDown(int key) => (GetAsyncKeyState(key) & 0x8000) != 0;

    private delegate nint HookProc(int code, nint message, nint data);

    [StructLayout(LayoutKind.Sequential)]
    private struct NativePoint { public int X; public int Y; }

    [StructLayout(LayoutKind.Sequential)]
    private struct MouseHookData
    {
        public NativePoint Position;
        public uint MouseData;
        public uint Flags;
        public uint Time;
        public nint ExtraInfo;
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern nint SetWindowsHookEx(int hook, HookProc callback, nint module, uint threadId);
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(nint hook);
    [DllImport("user32.dll")]
    private static extern nint CallNextHookEx(nint hook, int code, nint message, nint data);
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    private static extern nint GetModuleHandle(string? name);
    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int key);
    [DllImport("user32.dll")]
    private static extern nint WindowFromPoint(NativePoint point);
    [DllImport("user32.dll")]
    private static extern nint GetAncestor(nint window, int flags);
}
