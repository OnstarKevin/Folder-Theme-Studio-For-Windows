using FolderThemeStudio.Core.SystemIntegration;
using FolderThemeStudio.Core.Recovery;
using FolderThemeStudio.Core.Tests.TestSupport;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Xunit;

namespace FolderThemeStudio.Core.Tests.SystemIntegration;

public sealed class GlobalIconServiceTests
{
    [Fact]
    public void CaptureThenRestore_PreservesRawExpandStringKindWithoutExpansion()
    {
        var registry = new InMemoryRegistryStore();
        registry.SetRaw("3", @"%SystemRoot%\system32\shell32.dll,3", RegistryValueKind.ExpandString);
        var service = GlobalIconService.CreateForTesting(
            registry,
            ShellRefreshService.CreateForTesting(new FakeShellChangeNotifier()));

        var captured = service.CaptureSnapshot();
        Assert.True(captured.Success);
        Assert.Equal(@"%SystemRoot%\system32\shell32.dll,3", captured.RegistryValue3!.Value);
        Assert.Equal(RegistryValueKind.ExpandString, captured.RegistryValue3.Kind);

        Assert.True(service.ApplyWithoutRefresh(@"C:\Themes\ice.ico").Success);
        Assert.True(service.RestoreWithoutRefresh(new OperationSnapshot(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            captured.RegistryValue3,
            captured.RegistryValue4!,
            [],
            false)).Success);

        Assert.Equal(@"%SystemRoot%\system32\shell32.dll,3", registry.Get("3"));
        Assert.Equal(RegistryValueKind.ExpandString, registry.GetKind("3"));
    }
    [Fact]
    public void Apply_WritesClosedAndOpenFolderMappings()
    {
        var registry = new InMemoryRegistryStore();
        var service = GlobalIconService.CreateForTesting(registry, ShellRefreshService.CreateForTesting(new FakeShellChangeNotifier()));

        var result = service.Apply(@"C:\Themes\ice.ico");

        Assert.True(result.Success);
        Assert.Equal(@"C:\Themes\ice.ico,0", registry.Get("3"));
        Assert.Equal(@"C:\Themes\ice.ico,0", registry.Get("4"));
    }

    [Fact]
    public void Restore_ReinstatesMissingAndExistingValuesExactly()
    {
        var registry = new InMemoryRegistryStore();
        registry.Seed("3", "theme.ico,0");
        registry.Seed("4", "theme.ico,0");
        var service = GlobalIconService.CreateForTesting(registry, ShellRefreshService.CreateForTesting(new FakeShellChangeNotifier()));

        var result = service.Restore(SnapshotFactory.MixedRegistryState());

        Assert.True(result.Success);
        Assert.False(registry.Exists("3"));
        Assert.Equal("original.dll,4", registry.Get("4"));
    }

    [Fact]
    public void CaptureThenRestore_PreservesMixedExistingAndMissingRegistryValues()
    {
        var registry = new InMemoryRegistryStore();
        registry.Seed("4", "original.dll,4");
        var service = GlobalIconService.CreateForTesting(registry, ShellRefreshService.CreateForTesting(new FakeShellChangeNotifier()));

        var snapshot = service.CaptureSnapshot();
        var apply = service.Apply(@"C:\Themes\ice.ico");
        var restore = service.Restore(new(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            snapshot.RegistryValue3!,
            snapshot.RegistryValue4!,
            [],
            false));

        Assert.True(snapshot.Success);
        Assert.False(snapshot.RegistryValue3!.Existed);
        Assert.True(snapshot.RegistryValue4!.Existed);
        Assert.True(apply.Success);
        Assert.True(restore.Success);
        Assert.False(registry.Exists("3"));
        Assert.Equal("original.dll,4", registry.Get("4"));
    }

    [Fact]
    public void Apply_FailsWhenRegistryDoesNotRetainMappings()
    {
        var registry = new InMemoryRegistryStore { IgnoreWrites = true };
        var service = GlobalIconService.CreateForTesting(registry, ShellRefreshService.CreateForTesting(new FakeShellChangeNotifier()));

        var result = service.Apply(@"C:\Themes\ice.ico");

        Assert.False(result.Success);
        Assert.False(result.ShellRefresh.Attempted);
    }

    [Fact]
    public void Apply_ReportsRefreshFailureSeparatelyFromRegistrySuccess()
    {
        var registry = new InMemoryRegistryStore();
        var service = GlobalIconService.CreateForTesting(registry, ShellRefreshService.CreateForTesting(new FakeShellChangeNotifier(new ExternalException("shell notification failed"))));

        var result = service.Apply(@"C:\Themes\ice.ico");

        Assert.True(result.Success);
        Assert.True(result.ShellRefresh.Attempted);
        Assert.False(result.ShellRefresh.Success);
        Assert.Equal(@"C:\Themes\ice.ico,0", registry.Get("3"));
        Assert.Equal(@"C:\Themes\ice.ico,0", registry.Get("4"));
    }

    [Fact]
    public void Apply_ReturnsCapturedStateWhenSecondRegistryWriteFails()
    {
        var registry = new InMemoryRegistryStore { FailWhenSetting = "4" };
        registry.Seed("3", "closed-original.dll,3");
        registry.Seed("4", "open-original.dll,4");
        var service = GlobalIconService.CreateForTesting(registry, ShellRefreshService.CreateForTesting(new FakeShellChangeNotifier()));

        var result = service.Apply(@"C:\Themes\ice.ico");

        Assert.False(result.Success);
        Assert.Equal(new("3", true, "closed-original.dll,3", RegistryValueKind.String), result.RegistryValue3);
        Assert.Equal(new("4", true, "open-original.dll,4", RegistryValueKind.String), result.RegistryValue4);
        Assert.False(result.ShellRefresh.Attempted);
    }

    private sealed class FakeShellChangeNotifier : IShellChangeNotifier
    {
        private readonly Exception? exception;

        public FakeShellChangeNotifier(Exception? exception = null)
        {
            this.exception = exception;
        }

        public void NotifyAssociationChanged()
        {
            if (exception is not null)
            {
                throw exception;
            }
        }
    }
}
