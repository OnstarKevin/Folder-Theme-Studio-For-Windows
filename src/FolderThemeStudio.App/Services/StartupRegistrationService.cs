using Microsoft.Win32;
using System.IO;

namespace FolderThemeStudio.App.Services;

public interface IStartupRegistrationService
{
    void SetEnabled(bool enabled);
}

public interface IStartupRegistry
{
    void SetValue(string name, string value);
    void DeleteValue(string name);
}

public sealed class CurrentUserStartupRegistry : IStartupRegistry
{
    private const string RunKey = @"Software\Microsoft\Windows\CurrentVersion\Run";

    public void SetValue(string name, string value)
    {
        using var key = Registry.CurrentUser.CreateSubKey(RunKey, writable: true);
        key?.SetValue(name, value, RegistryValueKind.String);
    }

    public void DeleteValue(string name)
    {
        using var key = Registry.CurrentUser.OpenSubKey(RunKey, writable: true);
        key?.DeleteValue(name, throwOnMissingValue: false);
    }
}

public sealed class StartupRegistrationService : IStartupRegistrationService
{
    public const string ValueName = "FolderThemeStudio";
    private readonly IStartupRegistry registry;
    private readonly Func<string> executablePath;

    public StartupRegistrationService(IStartupRegistry? registry = null, Func<string>? executablePath = null)
    {
        this.registry = registry ?? new CurrentUserStartupRegistry();
        this.executablePath = executablePath ?? (() => Environment.ProcessPath
            ?? throw new InvalidOperationException("Cannot determine executable path."));
    }

    public void SetEnabled(bool enabled)
    {
        if (enabled)
        {
            registry.SetValue(ValueName, $"\"{Path.GetFullPath(executablePath())}\" --background");
        }
        else
        {
            registry.DeleteValue(ValueName);
        }
    }
}
