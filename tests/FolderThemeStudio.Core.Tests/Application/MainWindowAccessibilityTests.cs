using System.IO;
using System.Xml.Linq;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class MainWindowAccessibilityTests
{
    [Fact]
    public void AccessKeys_AreUniqueLettersRatherThanNumericPlaceholders()
    {
        var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
        var xamlPath = Path.Combine(projectRoot, "src", "FolderThemeStudio.App", "MainWindow.xaml");
        var document = XDocument.Load(xamlPath);
        var keys = document
            .Descendants()
            .Select(element => element.Attribute("Content")?.Value)
            .Where(content => content is not null && content.Contains('_'))
            .Select(content => char.ToUpperInvariant(content![content.IndexOf('_') + 1]))
            .ToArray();

        Assert.All(keys, key => Assert.True(char.IsLetter(key), $"Access key '{key}' is not a letter."));
        Assert.Equal(keys.Length, keys.Distinct().Count());
    }
}
