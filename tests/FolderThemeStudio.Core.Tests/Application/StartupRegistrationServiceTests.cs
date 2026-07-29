using FolderThemeStudio.App.Services;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class StartupRegistrationServiceTests
{
    [Fact]
    public void SetEnabled_WritesQuotedBackgroundCommand()
    {
        var registry = new RecordingRegistry();
        var service = new StartupRegistrationService(registry, () => @"C:\Program Files\Folder Theme\FolderThemeStudio.exe");

        service.SetEnabled(true);

        Assert.Equal(StartupRegistrationService.ValueName, registry.Name);
        Assert.Equal("\"C:\\Program Files\\Folder Theme\\FolderThemeStudio.exe\" --background", registry.Value);
    }

    [Fact]
    public void SetEnabledFalse_RemovesStartupValue()
    {
        var registry = new RecordingRegistry();
        var service = new StartupRegistrationService(registry, () => "app.exe");

        service.SetEnabled(false);

        Assert.Equal(StartupRegistrationService.ValueName, registry.DeletedName);
    }

    private sealed class RecordingRegistry : IStartupRegistry
    {
        public string? Name { get; private set; }
        public string? Value { get; private set; }
        public string? DeletedName { get; private set; }
        public void SetValue(string name, string value) { Name = name; Value = value; }
        public void DeleteValue(string name) => DeletedName = name;
    }
}
