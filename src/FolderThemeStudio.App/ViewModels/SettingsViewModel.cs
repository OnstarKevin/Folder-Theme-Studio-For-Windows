using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using FolderThemeStudio.App.Localization;
using FolderThemeStudio.App.Settings;
using FolderThemeStudio.App.Services;
using FolderThemeStudio.Core.Themes;

namespace FolderThemeStudio.App.ViewModels;

public sealed record PaletteRole(string DisplayName, ColorPickerViewModel Picker);

public sealed class SettingsViewModel : INotifyPropertyChanged
{
    private readonly AppSettings initial;
    private readonly IAppSettingsStore store;
    private readonly ILocalizationService localization;
    private readonly Action<FolderPalette> applyPalette;
    private readonly IPreviewRenderer previewRenderer;
    private string selectedLanguage;
    private PaletteRole? selectedRole;
    private BitmapSource? previewImage;
    private bool startWithWindows;
    private bool closeToTray;
    private double cardOpacityPercent;
    private FolderEditChord editChord;

    public SettingsViewModel(
        AppSettings initial,
        IAppSettingsStore store,
        ILocalizationService localization,
        Action<FolderPalette> applyPalette,
        IPreviewRenderer previewRenderer)
    {
        this.initial = initial ?? throw new ArgumentNullException(nameof(initial));
        this.store = store ?? throw new ArgumentNullException(nameof(store));
        this.localization = localization ?? throw new ArgumentNullException(nameof(localization));
        this.applyPalette = applyPalette ?? throw new ArgumentNullException(nameof(applyPalette));
        this.previewRenderer = previewRenderer ?? throw new ArgumentNullException(nameof(previewRenderer));
        selectedLanguage = initial.Language;
        startWithWindows = initial.StartWithWindows;
        closeToTray = initial.CloseToTray;
        cardOpacityPercent = initial.CardOpacity * 100;
        editChord = initial.EditChord;
        GradientStart = new ColorPickerViewModel(initial.Palette.GradientStart);
        GradientEnd = new ColorPickerViewModel(initial.Palette.GradientEnd);
        Stroke = new ColorPickerViewModel(initial.Palette.Stroke);
        Glow = new ColorPickerViewModel(initial.Palette.Glow);
        Roles =
        [
            new PaletteRole(localization.Get("Settings.GradientStart"), GradientStart),
            new PaletteRole(localization.Get("Settings.GradientEnd"), GradientEnd),
            new PaletteRole(localization.Get("Settings.Stroke"), Stroke),
            new PaletteRole(localization.Get("Settings.Glow"), Glow),
        ];
        selectedRole = Roles[0];
        RecentColors = new ObservableCollection<string>(initial.RecentColors);
        RecommendedPalettes =
        [
            FolderPalette.Default,
            new("#A78BFA", "#6366F1", "#DDD6FE", "#8B5CF6"),
            new("#34D399", "#059669", "#A7F3D0", "#10B981"),
            new("#FBBF24", "#F97316", "#FEF3C7", "#FB923C"),
            new("#FB7185", "#DB2777", "#FECDD3", "#F472B6"),
            new("#94A3B8", "#334155", "#E2E8F0", "#64748B"),
        ];
        foreach (var picker in Roles.Select(role => role.Picker)) picker.PropertyChanged += PickerChanged;
        RefreshPreview();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public ColorPickerViewModel GradientStart { get; }
    public ColorPickerViewModel GradientEnd { get; }
    public ColorPickerViewModel Stroke { get; }
    public ColorPickerViewModel Glow { get; }
    public IReadOnlyList<PaletteRole> Roles { get; }
    public IReadOnlyList<FolderPalette> RecommendedPalettes { get; }
    public ObservableCollection<string> RecentColors { get; }

    public string SelectedLanguage
    {
        get => selectedLanguage;
        set => SetField(ref selectedLanguage, value is "en-US" ? "en-US" : "zh-CN");
    }

    public PaletteRole? SelectedRole
    {
        get => selectedRole;
        set => SetField(ref selectedRole, value);
    }

    public BitmapSource? PreviewImage
    {
        get => previewImage;
        private set => SetField(ref previewImage, value);
    }

    public bool CanSave => Roles.All(role => role.Picker.IsValid);

    public bool StartWithWindows
    {
        get => startWithWindows;
        set => SetField(ref startWithWindows, value);
    }

    public bool CloseToTray
    {
        get => closeToTray;
        set => SetField(ref closeToTray, value);
    }

    public double CardOpacityPercent
    {
        get => cardOpacityPercent;
        set => SetField(ref cardOpacityPercent, Math.Clamp(value, 0, 100));
    }

    public FolderEditChord EditChord
    {
        get => editChord;
        set
        {
            if (!FolderEditChord.TryNormalize(value, out var normalized)) return;
            if (SetField(ref editChord, normalized)) OnPropertyChanged(nameof(EditChordDisplay));
        }
    }

    public string EditChordDisplay => EditChord.ToString();

    public async Task<bool> SaveAsync()
    {
        if (!CanSave) return false;
        var palette = CurrentPalette();
        var recent = new[] { palette.GradientStart, palette.GradientEnd, palette.Stroke, palette.Glow }
            .Concat(initial.RecentColors)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(12)
            .ToArray();
        var settings = new AppSettings(
            SelectedLanguage,
            initial.TutorialCompleted,
            palette,
            recent,
            StartWithWindows,
            CloseToTray,
            initial.MonitoringPaused,
            CardOpacityPercent / 100) { EditChord = EditChord };
        await store.SaveAsync(settings);
        localization.ApplyLanguage(SelectedLanguage);
        applyPalette(palette);
        return true;
    }

    public void Cancel() { }

    public void ApplyRecommended(FolderPalette palette)
    {
        GradientStart.TrySetHex(palette.GradientStart);
        GradientEnd.TrySetHex(palette.GradientEnd);
        Stroke.TrySetHex(palette.Stroke);
        Glow.TrySetHex(palette.Glow);
    }

    public void ApplyRecent(string color)
    {
        SelectedRole?.Picker.TrySetHex(color);
    }

    private FolderPalette CurrentPalette() => new(GradientStart.Hex, GradientEnd.Hex, Stroke.Hex, Glow.Hex);

    private void PickerChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(CanSave));
        if (e.PropertyName == nameof(ColorPickerViewModel.Hex) && CanSave) RefreshPreview();
    }

    private void RefreshPreview()
    {
        var palette = CurrentPalette();
        var theme = FolderTheme.IceBlue with
        {
            GradientStart = palette.GradientStart,
            GradientEnd = palette.GradientEnd,
            StrokeColor = palette.Stroke,
            GlowColor = palette.Glow,
        };
        PreviewImage = previewRenderer.Render(FolderThemeStudio.Core.Application.IconSource.BuiltIn(theme), 256);
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
