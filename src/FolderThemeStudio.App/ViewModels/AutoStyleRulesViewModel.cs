using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FolderThemeStudio.App.Localization;
using FolderThemeStudio.App.Monitoring;
using FolderThemeStudio.Core.Rendering;
using FolderThemeStudio.Core.Themes;

namespace FolderThemeStudio.App.ViewModels;

public sealed class AutoStyleRulesViewModel : INotifyPropertyChanged
{
    private readonly IMonitoringRuleStore store;
    private readonly IFolderMonitoringCoordinator monitoring;
    private readonly IMonitoringIconAssetService assets;
    private readonly IMainViewModelDialogs dialogs;
    private readonly ILocalizationService localization;
    private readonly IViewModelDispatcher dispatcher;
    private readonly FolderTheme editorTheme;
    private readonly List<MonitoringRule> roots = [];
    private string? selectedRootPath;
    private AutoStyleRuleItem? selectedRule;
    private string keyword = string.Empty;
    private string? selectedImagePath;
    private BitmapSource? iconPreview;
    private string statusKey = "AutoRules.Status.Ready";
    private bool useEditorTheme = true;
    private bool enabled = true;
    private bool isBusy;
    private string? editingRuleId;

    public AutoStyleRulesViewModel(
        IMonitoringRuleStore store,
        IFolderMonitoringCoordinator monitoring,
        IMonitoringIconAssetService assets,
        IMainViewModelDialogs dialogs,
        ILocalizationService localization,
        IViewModelDispatcher dispatcher,
        FolderTheme editorTheme)
    {
        this.store = store ?? throw new ArgumentNullException(nameof(store));
        this.monitoring = monitoring ?? throw new ArgumentNullException(nameof(monitoring));
        this.assets = assets ?? throw new ArgumentNullException(nameof(assets));
        this.dialogs = dialogs ?? throw new ArgumentNullException(nameof(dialogs));
        this.localization = localization ?? throw new ArgumentNullException(nameof(localization));
        this.dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        this.editorTheme = editorTheme ?? throw new ArgumentNullException(nameof(editorTheme));

        AddNewRuleCommand = new RelayCommand(_ => BeginNewRule());
        SaveRuleCommand = new RelayCommand(_ => _ = SaveRuleAsync(), _ => CanSaveRule);
        CancelEditCommand = new RelayCommand(_ => CancelEdit());
        DeleteRuleCommand = new RelayCommand(parameter => _ = DeleteRuleAsync(parameter as string), parameter => parameter is string);
        MoveUpCommand = new RelayCommand(parameter => _ = MoveRuleAsync(parameter as string, -1), parameter => CanMove(parameter as string, -1));
        MoveDownCommand = new RelayCommand(parameter => _ = MoveRuleAsync(parameter as string, 1), parameter => CanMove(parameter as string, 1));
        ToggleEnabledCommand = new RelayCommand(parameter => _ = ToggleRuleAsync(parameter as string), parameter => parameter is string);
        ChooseImageCommand = new RelayCommand(_ => _ = ChooseImageAsync());
        localization.LanguageChanged += LanguageChanged;
        UpdatePreview();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<string> RootPaths { get; } = [];
    public ObservableCollection<AutoStyleRuleItem> Rules { get; } = [];
    public ICommand AddNewRuleCommand { get; }
    public ICommand SaveRuleCommand { get; }
    public ICommand CancelEditCommand { get; }
    public ICommand DeleteRuleCommand { get; }
    public ICommand MoveUpCommand { get; }
    public ICommand MoveDownCommand { get; }
    public ICommand ToggleEnabledCommand { get; }
    public ICommand ChooseImageCommand { get; }

    public string? SelectedRootPath
    {
        get => selectedRootPath;
        set
        {
            if (!SetField(ref selectedRootPath, value)) return;
            LoadSelectedRootRules();
            OnPropertyChanged(nameof(FallbackIconName));
            RefreshCommands();
        }
    }

    public AutoStyleRuleItem? SelectedRule
    {
        get => selectedRule;
        set
        {
            if (!SetField(ref selectedRule, value)) return;
            if (value is null) return;
            editingRuleId = value.Id;
            Keyword = value.Keyword;
            enabled = value.Enabled;
            OnPropertyChanged(nameof(Enabled));
            UseEditorTheme = false;
            SelectedImagePath = value.IcoPath;
            UpdatePreview();
            RefreshCommands();
        }
    }

    public string Keyword
    {
        get => keyword;
        set
        {
            if (SetField(ref keyword, value)) RefreshCommands();
        }
    }

    public bool Enabled
    {
        get => enabled;
        set => SetField(ref enabled, value);
    }

    public bool UseEditorTheme
    {
        get => useEditorTheme;
        set
        {
            if (!SetField(ref useEditorTheme, value)) return;
            UpdatePreview();
            RefreshCommands();
        }
    }

    public string? SelectedImagePath
    {
        get => selectedImagePath;
        private set
        {
            if (SetField(ref selectedImagePath, value))
            {
                if (!UseEditorTheme) UpdatePreview();
                RefreshCommands();
            }
        }
    }

    public BitmapSource? IconPreview
    {
        get => iconPreview;
        private set => SetField(ref iconPreview, value);
    }

    public string StatusText => localization.Get(statusKey);
    public string FallbackIconName =>
        Path.GetFileName(roots.FirstOrDefault(item => string.Equals(item.RootPath, SelectedRootPath, StringComparison.OrdinalIgnoreCase))?.IcoPath)
        ?? localization.Get("AutoRules.Status.NoRoots");
    public bool CanSaveRule => !IsBusy && !string.IsNullOrWhiteSpace(SelectedRootPath) &&
                               !string.IsNullOrWhiteSpace(Keyword) && (UseEditorTheme || !string.IsNullOrWhiteSpace(SelectedImagePath));
    public bool CanEdit => !IsBusy;
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
            var loaded = await store.LoadAsync(token).ConfigureAwait(false);
            roots.Clear();
            roots.AddRange(loaded.Rules.OrderBy(rule => rule.RootPath, StringComparer.OrdinalIgnoreCase));
            await dispatcher.InvokeAsync(() =>
            {
                RootPaths.Clear();
                foreach (var root in roots) RootPaths.Add(root.RootPath);
                SelectedRootPath = RootPaths.FirstOrDefault();
                statusKey = RootPaths.Count == 0 ? "AutoRules.Status.NoRoots" : "AutoRules.Status.Ready";
                OnPropertyChanged(nameof(StatusText));
            });
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or InvalidDataException)
        {
            await dispatcher.InvokeAsync(() => SetStatus("AutoRules.Status.Error"));
        }
        finally { await dispatcher.InvokeAsync(() => IsBusy = false); }
    }

    public void BeginNewRule()
    {
        editingRuleId = null;
        SelectedRule = null;
        Keyword = string.Empty;
        Enabled = true;
        SelectedImagePath = null;
        UseEditorTheme = true;
        RefreshCommands();
    }

    public void CancelEdit()
    {
        if (selectedRule is null)
        {
            BeginNewRule();
            return;
        }

        editingRuleId = selectedRule.Id;
        Keyword = selectedRule.Keyword;
        enabled = selectedRule.Enabled;
        OnPropertyChanged(nameof(Enabled));
        useEditorTheme = false;
        OnPropertyChanged(nameof(UseEditorTheme));
        selectedImagePath = selectedRule.IcoPath;
        OnPropertyChanged(nameof(SelectedImagePath));
        UpdatePreview();
        RefreshCommands();
    }

    public async Task ChooseImageAsync(CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();
        var path = await dialogs.PickImageImportPathAsync().ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(path)) return;
        await dispatcher.InvokeAsync(() =>
        {
            UseEditorTheme = false;
            SelectedImagePath = path;
            SetStatus("AutoRules.Status.ImageSelected");
        });
    }

    public async Task<bool> SaveRuleAsync(CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(SelectedRootPath))
        {
            SetStatus("AutoRules.Status.NoRoots");
            return false;
        }
        if (string.IsNullOrWhiteSpace(Keyword))
        {
            SetStatus("AutoRules.Status.KeywordRequired");
            return false;
        }
        if (!UseEditorTheme && string.IsNullOrWhiteSpace(SelectedImagePath))
        {
            SetStatus("AutoRules.Status.ImageRequired");
            return false;
        }

        var rootPath = SelectedRootPath!;
        var root = roots.FirstOrDefault(item => string.Equals(item.RootPath, rootPath, StringComparison.OrdinalIgnoreCase));
        if (root is null)
        {
            SetStatus("AutoRules.Status.NoRoots");
            return false;
        }
        var request = new SaveRuleRequest(
            rootPath,
            root,
            editingRuleId ?? Guid.NewGuid().ToString("N"),
            Keyword.Trim(),
            Enabled,
            UseEditorTheme,
            SelectedImagePath,
            Rules.Select(item => item.ToRule()).ToList());

        IsBusy = true;
        try
        {
            var original = request.Rules.FirstOrDefault(item => item.Id == request.RuleId);
            var reuseOriginalIcon = original is not null && !request.UseEditorTheme &&
                !string.IsNullOrWhiteSpace(request.SelectedImagePath) &&
                File.Exists(original.IcoPath) && PathsEqual(request.SelectedImagePath, original.IcoPath);
            var icoPath = reuseOriginalIcon
                ? original!.IcoPath
                : await PersistSelectedIconAsync(request.UseEditorTheme, request.SelectedImagePath, token).ConfigureAwait(false);

            var updatedRules = request.Rules.ToList();
            var savedRule = new FolderNameStyleRule(request.RuleId, request.Keyword, icoPath, request.Enabled, updatedRules.Count);
            var oldIndex = updatedRules.FindIndex(item => item.Id == request.RuleId);
            if (oldIndex >= 0) updatedRules[oldIndex] = savedRule;
            else updatedRules.Add(savedRule);
            updatedRules = updatedRules.Select((item, index) => item with { Order = index }).ToList();
            var updatedRoot = request.Root with { NameRules = updatedRules, UpdatedAtUtc = DateTimeOffset.UtcNow };

            await monitoring.ReplaceRuleAsync(updatedRoot, token).ConfigureAwait(false);
            await dispatcher.InvokeAsync(() =>
            {
                var rootIndex = roots.FindIndex(item => string.Equals(item.RootPath, request.RootPath, StringComparison.OrdinalIgnoreCase));
                if (rootIndex >= 0) roots[rootIndex] = updatedRoot;
                if (string.Equals(SelectedRootPath, request.RootPath, StringComparison.OrdinalIgnoreCase))
                {
                    LoadSelectedRootRules();
                    SelectedRule = Rules.FirstOrDefault(item => item.Id == request.RuleId);
                }
                SetStatus("AutoRules.Status.Saved");
            });
            return true;
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or InvalidDataException or ArgumentException or NotSupportedException)
        {
            await dispatcher.InvokeAsync(() => SetStatus("AutoRules.Status.Error"));
            return false;
        }
        finally { await dispatcher.InvokeAsync(() => IsBusy = false); }
    }

    public async Task DeleteRuleAsync(string? id, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(id)) return;
        var index = Rules.ToList().FindIndex(rule => rule.Id == id);
        if (index < 0) return;
        Rules.RemoveAt(index);
        OnPropertyChanged(nameof(IsEmpty));
        if (editingRuleId == id) BeginNewRule();
        await NormalizeAndPersistAsync(token).ConfigureAwait(false);
        await dispatcher.InvokeAsync(() => SetStatus("AutoRules.Status.Deleted"));
    }

    public async Task MoveRuleAsync(string? id, int offset, CancellationToken token = default)
    {
        if (offset is not (-1 or 1) || string.IsNullOrWhiteSpace(id)) return;
        var index = Rules.ToList().FindIndex(rule => rule.Id == id);
        var target = index + offset;
        if (index < 0 || target < 0 || target >= Rules.Count) return;
        var item = Rules[index];
        Rules.RemoveAt(index);
        Rules.Insert(target, item);
        await NormalizeAndPersistAsync(token).ConfigureAwait(false);
    }

    public async Task ToggleRuleAsync(string? id, CancellationToken token = default)
    {
        var item = Rules.FirstOrDefault(rule => rule.Id == id);
        if (item is null) return;
        item.Enabled = !item.Enabled;
        await NormalizeAndPersistAsync(token).ConfigureAwait(false);
    }

    private async Task NormalizeAndPersistAsync(CancellationToken token)
    {
        MonitoringRule? updated = null;
        int rootIndex = -1;
        await dispatcher.InvokeAsync(() =>
        {
            var root = roots.FirstOrDefault(item => string.Equals(item.RootPath, SelectedRootPath, StringComparison.OrdinalIgnoreCase));
            if (root is null) return;
            for (var index = 0; index < Rules.Count; index++) Rules[index].Order = index;
            updated = root with
            {
                NameRules = Rules.Select(item => item.ToRule()).ToArray(),
                UpdatedAtUtc = DateTimeOffset.UtcNow
            };
            rootIndex = roots.FindIndex(item => string.Equals(item.RootPath, updated.RootPath, StringComparison.OrdinalIgnoreCase));
        });
        if (updated is null) return;
        await monitoring.ReplaceRuleAsync(updated, token).ConfigureAwait(false);
        await dispatcher.InvokeAsync(() => roots[rootIndex] = updated);
    }

    private async Task<string> PersistSelectedIconAsync(bool useTheme, string? imagePath, CancellationToken token)
    {
        var frames = new Dictionary<int, byte[]>();
        foreach (var size in FolderIconRenderer.RequiredSizes)
        {
            token.ThrowIfCancellationRequested();
            var bitmap = useTheme
                ? FolderIconRenderer.Render(editorTheme, size)
                : ImportedIconRenderer.Render(imagePath!, size);
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            using var stream = new MemoryStream();
            encoder.Save(stream);
            frames.Add(size, stream.ToArray());
        }
        var ico = IcoEncoder.Encode(frames);
        var temporaryDirectory = Path.Combine(Path.GetTempPath(), "FolderThemeStudio", "auto-rules");
        Directory.CreateDirectory(temporaryDirectory);
        var temporaryPath = Path.Combine(temporaryDirectory, Guid.NewGuid().ToString("N") + ".ico");
        try
        {
            await File.WriteAllBytesAsync(temporaryPath, ico, token).ConfigureAwait(false);
            return await assets.PersistAsync(temporaryPath, token).ConfigureAwait(false);
        }
        finally { if (File.Exists(temporaryPath)) File.Delete(temporaryPath); }
    }

    private void LoadSelectedRootRules()
    {
        Rules.Clear();
        selectedRule = null;
        editingRuleId = null;
        var root = roots.FirstOrDefault(item => string.Equals(item.RootPath, SelectedRootPath, StringComparison.OrdinalIgnoreCase));
        if (root is not null)
        {
            foreach (var rule in root.NameRules.OrderBy(item => item.Order))
                Rules.Add(AutoStyleRuleItem.FromRule(rule, LoadIcoPreview(rule.IcoPath)));
        }
        BeginNewRule();
        OnPropertyChanged(nameof(IsEmpty));
        RefreshCommands();
    }

    private bool CanMove(string? id, int offset)
    {
        var index = Rules.ToList().FindIndex(rule => rule.Id == id);
        return index >= 0 && index + offset >= 0 && index + offset < Rules.Count;
    }

    public bool IsEmpty => Rules.Count == 0;

    private void UpdatePreview()
    {
        try
        {
            var bitmap = UseEditorTheme
                ? FolderIconRenderer.Render(editorTheme, 64)
                : string.IsNullOrWhiteSpace(SelectedImagePath) ? null : ImportedIconRenderer.Render(SelectedImagePath, 64);
            IconPreview = bitmap;
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or ArgumentException or NotSupportedException)
        {
            IconPreview = null;
        }
    }

    private static BitmapSource? LoadIcoPreview(string path)
    {
        if (!File.Exists(path)) return null;
        try
        {
            using var stream = File.OpenRead(path);
            var decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            var frame = decoder.Frames[0];
            frame.Freeze();
            return frame;
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or NotSupportedException or ArgumentException)
        {
            return null;
        }
    }

    private static bool PathsEqual(string left, string right)
    {
        try
        {
            return string.Equals(Path.GetFullPath(left), Path.GetFullPath(right), StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception exception) when (exception is ArgumentException or IOException or NotSupportedException)
        {
            return false;
        }
    }

    private void SetStatus(string key)
    {
        statusKey = key;
        OnPropertyChanged(nameof(StatusText));
    }

    private void LanguageChanged(object? sender, EventArgs e) => OnPropertyChanged(nameof(StatusText));

    private void RefreshCommands()
    {
        OnPropertyChanged(nameof(CanSaveRule));
        OnPropertyChanged(nameof(CanEdit));
        (SaveRuleCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (MoveUpCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (MoveDownCommand as RelayCommand)?.RaiseCanExecuteChanged();
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

    private sealed record SaveRuleRequest(
        string RootPath,
        MonitoringRule Root,
        string RuleId,
        string Keyword,
        bool Enabled,
        bool UseEditorTheme,
        string? SelectedImagePath,
        IReadOnlyList<FolderNameStyleRule> Rules);
}

public sealed class AutoStyleRuleItem : INotifyPropertyChanged
{
    private bool enabled;
    private int order;

    public AutoStyleRuleItem(string id, string keyword, string icoPath, bool enabled, int order, BitmapSource? preview)
    {
        Id = id;
        Keyword = keyword;
        IcoPath = icoPath;
        this.enabled = enabled;
        this.order = order;
        Preview = preview;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public string Id { get; }
    public string Keyword { get; }
    public string IcoPath { get; }
    public string IconName => Path.GetFileName(IcoPath);
    public BitmapSource? Preview { get; }
    public bool Enabled { get => enabled; set { if (enabled != value) { enabled = value; PropertyChanged?.Invoke(this, new(nameof(Enabled))); } } }
    public int Order { get => order; set { if (order != value) { order = value; PropertyChanged?.Invoke(this, new(nameof(Order))); } } }

    internal FolderNameStyleRule ToRule() => new(Id, Keyword, IcoPath, Enabled, Order);
    internal static AutoStyleRuleItem FromRule(FolderNameStyleRule rule, BitmapSource? preview) =>
        new(rule.Id, rule.Keyword, rule.IcoPath, rule.Enabled, rule.Order, preview);
}
