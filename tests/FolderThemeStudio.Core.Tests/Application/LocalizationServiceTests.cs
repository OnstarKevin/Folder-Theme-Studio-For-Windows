using FolderThemeStudio.App.Localization;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class LocalizationServiceTests
{
    [Fact]
    public void ApplyLanguage_ChangesSelectedTextAtRuntime()
    {
        var service = new LocalizationService("zh-CN", applyToApplication: false);

        Assert.Equal("文件夹主题工坊", service.Get("App.Title"));

        service.ApplyLanguage("en-US");

        Assert.Equal("Folder Theme Studio", service.Get("App.Title"));
        Assert.Equal("en-US", service.CurrentLanguage);
    }

    [Fact]
    public void ApplyLanguage_UnsupportedLanguage_FallsBackToChinese()
    {
        var service = new LocalizationService("en-US", applyToApplication: false);

        service.ApplyLanguage("fr-FR");

        Assert.Equal("zh-CN", service.CurrentLanguage);
        Assert.Equal("设置", service.Get("Top.Settings"));
    }

    [Fact]
    public void Get_FormatsSelectedLanguageTemplate()
    {
        var service = new LocalizationService("en-US", applyToApplication: false);

        Assert.Equal("Allowed: 3", service.Get("Plan.Allowed", 3));
    }
}
