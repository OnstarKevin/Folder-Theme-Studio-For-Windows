using FolderThemeStudio.Core.SystemIntegration;
using Microsoft.Win32;

namespace FolderThemeStudio.Core.Tests.TestSupport;

internal sealed class InMemoryRegistryStore : IRegistryStore
{
    private readonly Dictionary<string, RegistryStoreValue> values = new(StringComparer.Ordinal);

    public bool TryGet(string name, out RegistryStoreValue? value) => values.TryGetValue(name, out value);

    internal bool IgnoreWrites { get; set; }
    internal string? FailWhenSetting { get; set; }

    public void Set(string name, string value, RegistryValueKind kind)
    {
        if (string.Equals(name, FailWhenSetting, StringComparison.Ordinal))
        {
            throw new UnauthorizedAccessException("simulated registry write failure");
        }

        if (!IgnoreWrites)
        {
            values[name] = new RegistryStoreValue(value, kind);
        }
    }

    public void Delete(string name) => values.Remove(name);

    internal void Seed(string name, string value) => values[name] = new RegistryStoreValue(value, RegistryValueKind.String);
    internal void SetRaw(string name, string value, RegistryValueKind kind) => Set(name, value, kind);

    internal bool Exists(string name) => values.ContainsKey(name);

    internal string Get(string name) => values[name].RawValue;
    internal RegistryValueKind GetKind(string name) => values[name].Kind;
}
