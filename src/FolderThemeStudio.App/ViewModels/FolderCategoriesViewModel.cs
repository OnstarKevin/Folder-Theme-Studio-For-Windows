using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using FolderThemeStudio.App.Localization;
using FolderThemeStudio.App.Services;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace FolderThemeStudio.App.ViewModels;

public sealed class FolderCategoriesViewModel : INotifyPropertyChanged, IDisposable
{
    private readonly IFolderCategoryCatalog catalog;
    private readonly IFolderHoverMonitorService monitor;
    private readonly IMainViewModelDialogs dialogs;
    private readonly ILocalizationService localization;
    private readonly IViewModelDispatcher dispatcher;
    private FolderCategoryEntry? selectedEntry;
    private string folderPath = string.Empty;
    private string category = string.Empty;
    private string note = string.Empty;
    private string statusKey = "FolderCategories.Status.Ready";
    private bool isBusy;

    public FolderCategoriesViewModel(
        IFolderCategoryCatalog catalog,
        IFolderHoverMonitorService monitor,
        IMainViewModelDialogs dialogs,
        ILocalizationService localization,
        IViewModelDispatcher dispatcher)
    {
        this.catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        this.monitor = monitor ?? throw new ArgumentNullException(nameof(monitor));
        this.dialogs = dialogs ?? throw new ArgumentNullException(nameof(dialogs));
        this.localization = localization ?? throw new ArgumentNullException(nameof(localization));
        this.dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        GradientStartPicker = new ColorPickerViewModel("#2868D7");
        GradientEndPicker = new ColorPickerViewModel("#2868D7");
        GradientStartPicker.PropertyChanged += PickerChanged;
        GradientEndPicker.PropertyChanged += PickerChanged;
        PresetColors = ["#2868D7", "#0F9D6C", "#BA4A37", "#9257C4", "#D28B00", "#D34E88"];
        PickFolderCommand = new RelayCommand(_ => _ = PickFolderAsync(), _ => !IsBusy);
        NewEntryCommand = new RelayCommand(_ => BeginNewEntry(), _ => !IsBusy);
        SaveCommand = new RelayCommand(_ => _ = SaveAsync(), _ => CanSave);
        RemoveCommand = new RelayCommand(_ => _ = RemoveAsync(), _ => !IsBusy && SelectedEntry is not null);
        localization.LanguageChanged += LanguageChanged;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<FolderCategoryEntry> Entries { get; } = [];
    public ObservableCollection<string> PresetColors { get; }
    public ColorPickerViewModel GradientStartPicker { get; }
    public ColorPickerViewModel GradientEndPicker { get; }
    public ICommand PickFolderCommand { get; }
    public ICommand NewEntryCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand RemoveCommand { get; }

    public void Dispose()
    {
        localization.LanguageChanged -= LanguageChanged;
        GradientStartPicker.PropertyChanged -= PickerChanged;
        GradientEndPicker.PropertyChanged -= PickerChanged;
    }

    public FolderCategoryEntry? SelectedEntry
    {
        get => selectedEntry;
        set
        {
            if (!SetField(ref selectedEntry, value)) return;
            if (value is null)
            {
                RefreshCommands();
                return;
            }
            FolderPath = value.FolderPath;
            Category = value.Category;
            Note = value.Note;
            GradientStartPicker.Hex = value.GradientStartHex;
            GradientEndPicker.Hex = value.GradientEndHex;
            RefreshCommands();
        }
    }

    public string FolderPath { get => folderPath; private set { if (SetField(ref folderPath, value)) { OnPropertyChanged(nameof(FolderPathDisplay)); RefreshCommands(); } } }
    public string FolderPathDisplay => string.IsNullOrWhiteSpace(FolderPath) ? localization.Get("FolderCategories.Form.NoFolder") : FolderPath;
    public string Category { get => category; set { if (SetField(ref category, value)) RefreshCommands(); } }
    public string Note { get => note; set { if (SetField(ref note, value)) RefreshCommands(); } }
    public string ColorHex
    {
        get => GradientStartPicker.Hex;
        set
        {
            GradientStartPicker.Hex = value;
            GradientEndPicker.Hex = value;
            OnPropertyChanged(nameof(AccentPreview));
            RefreshCommands();
        }
    }
    public Brush AccentPreview
    {
        get
        {
            try
            {
                return new LinearGradientBrush(
                    (Color)ColorConverter.ConvertFromString(GradientStartPicker.Hex),
                    (Color)ColorConverter.ConvertFromString(GradientEndPicker.Hex),
                    new System.Windows.Point(0, 0), new System.Windows.Point(1, 1));
            }
            catch (Exception exception) when (exception is FormatException or ArgumentException or NotSupportedException)
            {
                return new SolidColorBrush(Color.FromRgb(40, 104, 215));
            }
        }
    }
    public string StatusText => localization.Get(statusKey);
    public bool IsEmpty => Entries.Count == 0;
    public bool CanEdit => !IsBusy;
    public bool CanSave => !IsBusy && !string.IsNullOrWhiteSpace(FolderPath) &&
                           !string.IsNullOrWhiteSpace(Category) && Category.Trim().Length <= 32 && Note.Trim().Length <= 140 &&
                           IsColorValid(GradientStartPicker.Hex) && IsColorValid(GradientEndPicker.Hex);
    public bool IsBusy
    {
        get => isBusy;
        private set
        {
            if (SetField(ref isBusy, value)) RefreshCommands();
        }
    }

    public async Task LoadAsync(CancellationToken token = default)
    {
        IsBusy = true;
        try
        {
            var entries = await catalog.LoadAsync(token).ConfigureAwait(false);
            await dispatcher.InvokeAsync(() =>
            {
                ReplaceEntries(entries);
                SetStatus("FolderCategories.Status.Ready");
            });
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or InvalidDataException or ArgumentException)
        {
            await dispatcher.InvokeAsync(() => SetStatus("FolderCategories.Status.Error"));
        }
        finally { IsBusy = false; }
    }

    public async Task PickFolderAsync(CancellationToken token = default)
    {
        if (IsBusy) return;
        var path = await dialogs.PickFolderAsync();
        if (string.IsNullOrWhiteSpace(path)) return;
        SelectFolder(path);
    }

    public void SelectFolder(string path)
    {
        var existing = Entries.FirstOrDefault(item => string.Equals(item.FolderPath, Path.GetFullPath(path), StringComparison.OrdinalIgnoreCase));
        if (existing is not null)
        {
            SelectedEntry = existing;
            SetStatus("FolderCategories.Status.Selected");
            return;
        }

        SelectedEntry = null;
        FolderPath = Path.GetFullPath(path);
        Category = string.Empty;
        Note = string.Empty;
        ColorHex = PresetColors[0];
        SetStatus("FolderCategories.Status.New");
    }

    public void BeginNewEntry()
    {
        SelectedEntry = null;
        FolderPath = string.Empty;
        Category = string.Empty;
        Note = string.Empty;
        ColorHex = PresetColors[0];
        SetStatus("FolderCategories.Status.Ready");
    }

    public void ChoosePresetColor(string? color)
    {
        if (color is not null && PresetColors.Contains(color, StringComparer.OrdinalIgnoreCase)) ColorHex = color;
    }

    public async Task<bool> SaveAsync(CancellationToken token = default)
    {
        if (!CanSave) return false;
        IsBusy = true;
        try
        {
            var entry = new FolderCategoryEntry(FolderPath, Category.Trim(), Note.Trim(), GradientStartPicker.Hex.Trim(), GradientEndPicker.Hex.Trim());
            await catalog.UpsertAsync(entry, token).ConfigureAwait(false);
            var updated = await catalog.LoadAsync(token).ConfigureAwait(false);
            await monitor.ReloadCatalogAsync(token).ConfigureAwait(false);
            await dispatcher.InvokeAsync(() =>
            {
                ReplaceEntries(updated);
                SelectedEntry = Entries.FirstOrDefault(item => string.Equals(item.FolderPath, entry.FolderPath, StringComparison.OrdinalIgnoreCase));
                SetStatus("FolderCategories.Status.Saved");
            });
            return true;
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or InvalidDataException or ArgumentException)
        {
            await dispatcher.InvokeAsync(() => SetStatus("FolderCategories.Status.Error"));
            return false;
        }
        finally { await dispatcher.InvokeAsync(() => IsBusy = false); }
    }

    public async Task RemoveAsync(CancellationToken token = default)
    {
        if (IsBusy || SelectedEntry is not { } selected) return;
        IsBusy = true;
        try
        {
            await catalog.RemoveAsync(selected.FolderPath, token).ConfigureAwait(false);
            var updated = await catalog.LoadAsync(token).ConfigureAwait(false);
            await monitor.ReloadCatalogAsync(token).ConfigureAwait(false);
            await dispatcher.InvokeAsync(() =>
            {
                ReplaceEntries(updated);
                BeginNewEntry();
                SetStatus("FolderCategories.Status.Removed");
            });
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or InvalidDataException or ArgumentException)
        {
            await dispatcher.InvokeAsync(() => SetStatus("FolderCategories.Status.Error"));
        }
        finally { await dispatcher.InvokeAsync(() => IsBusy = false); }
    }

    private void ReplaceEntries(IEnumerable<FolderCategoryEntry> entries)
    {
        Entries.Clear();
        foreach (var entry in entries.OrderBy(item => item.FolderPath, StringComparer.OrdinalIgnoreCase)) Entries.Add(entry);
        OnPropertyChanged(nameof(IsEmpty));
    }

    private static bool IsColorValid(string value) => value.Length == 7 && value[0] == '#' && value.AsSpan(1).ToString().All(Uri.IsHexDigit);

    private void SetStatus(string key)
    {
        statusKey = key;
        OnPropertyChanged(nameof(StatusText));
    }

    private void LanguageChanged(object? sender, EventArgs e) => OnPropertyChanged(nameof(StatusText));

    private void PickerChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(ColorPickerViewModel.Hex)) return;
        OnPropertyChanged(nameof(ColorHex));
        OnPropertyChanged(nameof(AccentPreview));
        RefreshCommands();
    }

    private void RefreshCommands()
    {
        OnPropertyChanged(nameof(CanSave));
        OnPropertyChanged(nameof(CanEdit));
        (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (PickFolderCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (NewEntryCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (RemoveCommand as RelayCommand)?.RaiseCanExecuteChanged();
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(name);
        return true;
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
