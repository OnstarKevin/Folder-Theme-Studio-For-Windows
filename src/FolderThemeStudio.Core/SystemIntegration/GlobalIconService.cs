using FolderThemeStudio.Core.Recovery;
using Microsoft.Win32;

namespace FolderThemeStudio.Core.SystemIntegration;

public sealed record GlobalApplyResult(
    bool Success,
    RegistryValueSnapshot? RegistryValue3,
    RegistryValueSnapshot? RegistryValue4,
    ShellRefreshResult ShellRefresh,
    string? Error);

public sealed record GlobalRestoreResult(bool Success, ShellRefreshResult ShellRefresh, string? Error);

public sealed record GlobalSnapshotCaptureResult(
    bool Success,
    RegistryValueSnapshot? RegistryValue3,
    RegistryValueSnapshot? RegistryValue4,
    string? Error);

internal interface IRegistryStore
{
    bool TryGet(string name, out RegistryStoreValue? value);
    void Set(string name, string value, RegistryValueKind kind);
    void Delete(string name);
}

internal sealed record RegistryStoreValue(string RawValue, RegistryValueKind Kind);

public sealed class GlobalIconService
{
    private const string ClosedFolderValueName = "3";
    private const string OpenFolderValueName = "4";
    private readonly IRegistryStore registry;
    private readonly ShellRefreshService shellRefresh;

    public GlobalIconService()
        : this(new WindowsRegistryStore(), new ShellRefreshService())
    {
    }

    internal static GlobalIconService CreateForTesting(IRegistryStore registry, ShellRefreshService shellRefresh) =>
        new(registry, shellRefresh);

    private GlobalIconService(IRegistryStore registry, ShellRefreshService shellRefresh)
    {
        this.registry = registry;
        this.shellRefresh = shellRefresh;
    }

    public GlobalApplyResult Apply(string icoPath)
        => ApplyCore(icoPath, refreshShell: true);

    public GlobalApplyResult ApplyWithoutRefresh(string icoPath)
        => ApplyCore(icoPath, refreshShell: false);

    public GlobalSnapshotCaptureResult CaptureSnapshot()
    {
        try
        {
            return new GlobalSnapshotCaptureResult(true, Capture(ClosedFolderValueName), Capture(OpenFolderValueName), null);
        }
        catch (Exception exception) when (exception is UnauthorizedAccessException or System.Security.SecurityException or System.IO.IOException or InvalidOperationException or ArgumentException)
        {
            return new GlobalSnapshotCaptureResult(false, null, null, exception.Message);
        }
    }

    private GlobalApplyResult ApplyCore(string icoPath, bool refreshShell)
    {
        if (string.IsNullOrWhiteSpace(icoPath))
        {
            return ApplyFailure(null, null, "An icon path is required.");
        }

        RegistryValueSnapshot? value3 = null;
        RegistryValueSnapshot? value4 = null;
        try
        {
            value3 = Capture(ClosedFolderValueName);
            value4 = Capture(OpenFolderValueName);
            var mapping = $"{icoPath},0";
            registry.Set(ClosedFolderValueName, mapping, RegistryValueKind.String);
            registry.Set(OpenFolderValueName, mapping, RegistryValueKind.String);

            if (!HasValue(ClosedFolderValueName, mapping) || !HasValue(OpenFolderValueName, mapping))
            {
                return ApplyFailure(value3, value4, "Windows did not retain the requested folder icon mappings.");
            }
        }
        catch (Exception exception) when (exception is UnauthorizedAccessException or System.Security.SecurityException or System.IO.IOException or InvalidOperationException or ArgumentException)
        {
            return ApplyFailure(value3, value4, exception.Message);
        }

        return new GlobalApplyResult(
            true,
            value3,
            value4,
            refreshShell ? shellRefresh.RefreshIcons() : NotAttemptedRefresh(),
            null);
    }

    public GlobalRestoreResult Restore(OperationSnapshot snapshot)
        => RestoreCore(snapshot, refreshShell: true);

    public GlobalRestoreResult RestoreWithoutRefresh(OperationSnapshot snapshot)
        => RestoreCore(snapshot, refreshShell: false);

    private GlobalRestoreResult RestoreCore(OperationSnapshot snapshot, bool refreshShell)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        snapshot.Validate();

        try
        {
            Restore(snapshot.RegistryValue3);
            Restore(snapshot.RegistryValue4);

            if (!Matches(snapshot.RegistryValue3) || !Matches(snapshot.RegistryValue4))
            {
                return new GlobalRestoreResult(false, NotAttemptedRefresh(), "Windows did not retain the restored folder icon mappings.");
            }
        }
        catch (Exception exception) when (exception is UnauthorizedAccessException or System.Security.SecurityException or System.IO.IOException or InvalidOperationException or ArgumentException)
        {
            return new GlobalRestoreResult(false, NotAttemptedRefresh(), exception.Message);
        }

        return new GlobalRestoreResult(
            true,
            refreshShell ? shellRefresh.RefreshIcons() : NotAttemptedRefresh(),
            null);
    }

    private RegistryValueSnapshot Capture(string name)
    {
        var existed = registry.TryGet(name, out var value);
        if (existed && value is null)
        {
            throw new InvalidOperationException($"Registry value '{name}' exists without a string value.");
        }

        return new RegistryValueSnapshot(name, existed, value?.RawValue, value?.Kind);
    }

    private bool HasValue(string name, string expected) =>
        registry.TryGet(name, out var actual)
        && actual is not null
        && actual.Kind == RegistryValueKind.String
        && string.Equals(actual.RawValue, expected, StringComparison.Ordinal);

    private void Restore(RegistryValueSnapshot value)
    {
        if (value.Existed)
        {
            registry.Set(value.Name, value.Value!, value.Kind!.Value);
        }
        else
        {
            registry.Delete(value.Name);
        }
    }

    private bool Matches(RegistryValueSnapshot expected)
    {
        var exists = registry.TryGet(expected.Name, out var actual);
        return expected.Existed
            ? exists
                && actual is not null
                && actual.Kind == expected.Kind
                && string.Equals(actual.RawValue, expected.Value, StringComparison.Ordinal)
            : !exists;
    }

    private static GlobalApplyResult ApplyFailure(RegistryValueSnapshot? value3, RegistryValueSnapshot? value4, string error) =>
        new(false, value3, value4, NotAttemptedRefresh(), error);

    private static ShellRefreshResult NotAttemptedRefresh() => new(false, false, null);
}

internal sealed class WindowsRegistryStore : IRegistryStore
{
    private const string ShellIconsKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Icons";

    public bool TryGet(string name, out RegistryStoreValue? value)
    {
        using var key = Registry.CurrentUser.OpenSubKey(ShellIconsKeyPath, writable: false);
        if (key is null)
        {
            value = null;
            return false;
        }

        var rawValue = key.GetValue(name, null, RegistryValueOptions.DoNotExpandEnvironmentNames);
        if (rawValue is null)
        {
            value = null;
            return false;
        }

        var kind = key.GetValueKind(name);
        if (kind is not (RegistryValueKind.String or RegistryValueKind.ExpandString))
        {
            throw new InvalidOperationException($"Registry value '{name}' is not REG_SZ or REG_EXPAND_SZ.");
        }

        value = new RegistryStoreValue(
            rawValue as string ?? throw new InvalidOperationException($"Registry value '{name}' is not a string."),
            kind);
        return true;
    }

    public void Set(string name, string value, RegistryValueKind kind)
    {
        using var key = Registry.CurrentUser.CreateSubKey(ShellIconsKeyPath, writable: true)
            ?? throw new InvalidOperationException("Could not open the current-user Shell Icons registry key.");
        if (kind is not (RegistryValueKind.String or RegistryValueKind.ExpandString))
        {
            throw new ArgumentOutOfRangeException(nameof(kind));
        }

        key.SetValue(name, value, kind);
    }

    public void Delete(string name)
    {
        using var key = Registry.CurrentUser.OpenSubKey(ShellIconsKeyPath, writable: true);
        key?.DeleteValue(name, throwOnMissingValue: false);
    }
}
