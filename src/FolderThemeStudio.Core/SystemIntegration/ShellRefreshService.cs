using System.Runtime.InteropServices;

namespace FolderThemeStudio.Core.SystemIntegration;

public sealed record ShellRefreshResult(bool Attempted, bool Success, string? Error);

internal interface IShellChangeNotifier
{
    void NotifyAssociationChanged();
}

public sealed class ShellRefreshService
{
    private readonly IShellChangeNotifier notifier;

    public ShellRefreshService()
        : this(new WindowsShellChangeNotifier())
    {
    }

    internal static ShellRefreshService CreateForTesting(IShellChangeNotifier notifier) => new(notifier);

    private ShellRefreshService(IShellChangeNotifier notifier)
    {
        this.notifier = notifier;
    }

    public ShellRefreshResult RefreshIcons()
    {
        try
        {
            notifier.NotifyAssociationChanged();
            return new ShellRefreshResult(true, true, null);
        }
        catch (Exception exception) when (exception is ExternalException or DllNotFoundException or EntryPointNotFoundException or InvalidOperationException)
        {
            return new ShellRefreshResult(true, false, exception.Message);
        }
    }
}

internal sealed class WindowsShellChangeNotifier : IShellChangeNotifier
{
    private const uint ShcneAssocChanged = 0x08000000;
    private const uint ShcnfIdList = 0x0000;

    public void NotifyAssociationChanged() => SHChangeNotify(ShcneAssocChanged, ShcnfIdList, IntPtr.Zero, IntPtr.Zero);

    [DllImport("shell32.dll")]
    private static extern void SHChangeNotify(uint eventId, uint flags, IntPtr item1, IntPtr item2);
}
