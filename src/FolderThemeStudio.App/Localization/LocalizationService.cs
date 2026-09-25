using System.Globalization;
using System.Windows;

namespace FolderThemeStudio.App.Localization;

public interface ILocalizationService
{
    string CurrentLanguage { get; }
    event EventHandler? LanguageChanged;
    void ApplyLanguage(string language);
    string Get(string key, params object[] arguments);
}

public sealed class LocalizationService : ILocalizationService
{
    private static readonly IReadOnlyDictionary<string, string> English = new Dictionary<string, string>
    {
        ["App.Title"] = "Folder Theme Studio",
        ["Top.Settings"] = "Settings",
        ["FolderCategories.Form.NoFolder"] = "No folder selected",
        ["FolderCategories.Status.Ready"] = "Choose or create a folder category.",
        ["FolderCategories.Status.Selected"] = "Existing category selected.",
        ["FolderCategories.Status.New"] = "Folder selected. Enter a category name.",
        ["FolderCategories.Status.Saved"] = "Category saved and hover card updated.",
        ["FolderCategories.Status.Removed"] = "Category removed.",
        ["FolderCategories.Status.Error"] = "Could not read or save categories. Check permissions or the local configuration file.",
        ["AutoRules.Status.Ready"] = "Add or edit name-match rules for monitored folders.",
        ["AutoRules.Status.NoRoots"] = "No monitored roots yet. Enable monitoring when applying a style first.",
        ["AutoRules.Status.KeywordRequired"] = "Enter a keyword before saving this rule.",
        ["AutoRules.Status.ImageRequired"] = "Choose an image or switch to the current editor style.",
        ["AutoRules.Status.ImageSelected"] = "Image selected. Save the rule to use it for new folders.",
        ["AutoRules.Status.Saved"] = "Rule saved and monitoring updated.",
        ["AutoRules.Status.Deleted"] = "Rule deleted.",
        ["AutoRules.Status.Error"] = "The rule could not be saved. Check the selected folder and icon file.",
        ["Plan.Allowed"] = "Allowed: {0}",
        ["Settings.GradientStart"] = "Gradient start",
        ["Settings.GradientEnd"] = "Gradient end",
        ["Settings.Stroke"] = "Stroke",
        ["Settings.Glow"] = "Glow",
        ["Tutorial.Step1.Title"] = "Choose a theme",
        ["Tutorial.Step1.Body"] = "Start with a built-in theme or a saved personal theme.",
        ["Tutorial.Step2.Title"] = "Tune the palette",
        ["Tutorial.Step2.Body"] = "Use Settings to adjust gradient, stroke, and glow colors visually.",
        ["Tutorial.Step3.Title"] = "Inspect the preview",
        ["Tutorial.Step3.Body"] = "Check small and large icons on light and dark backgrounds.",
        ["Tutorial.Step4.Title"] = "Apply safely",
        ["Tutorial.Step4.Body"] = "Calculate the plan, review it, then apply to ordinary folders.",
        ["Tutorial.Step5.Title"] = "Restore when needed",
        ["Tutorial.Step5.Body"] = "Restore the latest backup to undo the most recent application.",
    };

    private static readonly IReadOnlyDictionary<string, string> Chinese = new Dictionary<string, string>
    {
        ["App.Title"] = "文件夹主题工坊",
        ["Top.Settings"] = "设置",
        ["FolderCategories.Form.NoFolder"] = "尚未选择文件夹",
        ["FolderCategories.Status.Ready"] = "选择或新建一个文件夹分类。",
        ["FolderCategories.Status.Selected"] = "已选择现有分类。",
        ["FolderCategories.Status.New"] = "文件夹已选定，请填写分类名称。",
        ["FolderCategories.Status.Saved"] = "分类已保存，悬浮卡片已更新。",
        ["FolderCategories.Status.Removed"] = "分类已移除。",
        ["FolderCategories.Status.Error"] = "无法读取或保存分类，请检查权限或本地配置文件。",
        ["AutoRules.Status.Ready"] = "可为监控目录添加或编辑名称匹配规则。",
        ["AutoRules.Status.NoRoots"] = "还没有监控目录。请先在应用样式时启用新文件夹监控。",
        ["AutoRules.Status.KeywordRequired"] = "保存规则前请填写关键词。",
        ["AutoRules.Status.ImageRequired"] = "请选择图片，或切换为当前编辑样式。",
        ["AutoRules.Status.ImageSelected"] = "图片已选择；保存规则后即可用于新建文件夹。",
        ["AutoRules.Status.Saved"] = "规则已保存，监控已更新。",
        ["AutoRules.Status.Deleted"] = "规则已删除。",
        ["AutoRules.Status.Error"] = "规则保存失败，请检查监控目录和图标文件。",
        ["Plan.Allowed"] = "允许：{0}",
        ["Settings.GradientStart"] = "渐变起始色",
        ["Settings.GradientEnd"] = "渐变结束色",
        ["Settings.Stroke"] = "描边",
        ["Settings.Glow"] = "光晕",
        ["Tutorial.Step1.Title"] = "选择主题",
        ["Tutorial.Step1.Body"] = "从内置主题或已保存的个人主题开始。",
        ["Tutorial.Step2.Title"] = "调整配色",
        ["Tutorial.Step2.Body"] = "在设置中可视化调整渐变、描边和光晕颜色。",
        ["Tutorial.Step3.Title"] = "检查预览",
        ["Tutorial.Step3.Body"] = "在浅色和深色背景上检查大小图标。",
        ["Tutorial.Step4.Title"] = "安全应用",
        ["Tutorial.Step4.Body"] = "先计算并检查方案，再应用到普通文件夹。",
        ["Tutorial.Step5.Title"] = "需要时恢复",
        ["Tutorial.Step5.Body"] = "使用最近备份撤销上一次应用操作。",
    };

    private readonly bool applyToApplication;
    private IReadOnlyDictionary<string, string> text = Chinese;

    public LocalizationService(string language = "zh-CN", bool applyToApplication = true)
    {
        this.applyToApplication = applyToApplication;
        ApplyLanguage(language);
    }

    public string CurrentLanguage { get; private set; } = "zh-CN";
    public event EventHandler? LanguageChanged;

    public void ApplyLanguage(string language)
    {
        var selected = language is "en-US" ? "en-US" : "zh-CN";
        if (applyToApplication && System.Windows.Application.Current is not null)
        {
            var next = new ResourceDictionary
            {
                Source = new Uri($"/FolderThemeStudio.App;component/Localization/Strings.{selected}.xaml", UriKind.Relative),
            };
            var dictionaries = System.Windows.Application.Current.Resources.MergedDictionaries;
            var old = dictionaries.Where(item =>
                item.Source?.OriginalString.Contains("Localization/Strings.", StringComparison.Ordinal) == true).ToArray();
            foreach (var dictionary in old)
            {
                dictionaries.Remove(dictionary);
            }
            dictionaries.Add(next);
        }

        text = selected == "en-US" ? English : Chinese;
        CurrentLanguage = selected;
        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(selected);
        LanguageChanged?.Invoke(this, EventArgs.Empty);
    }

    public string Get(string key, params object[] arguments)
    {
        var template = text.TryGetValue(key, out var value) ? value : key;
        return arguments.Length == 0
            ? template
            : string.Format(CultureInfo.GetCultureInfo(CurrentLanguage), template, arguments);
    }
}
