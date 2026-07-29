using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FolderThemeStudio.App.Localization;
using FolderThemeStudio.App.Settings;

namespace FolderThemeStudio.App.ViewModels;

public sealed record TutorialStep(string Title, string Body);

public sealed class TutorialViewModel : INotifyPropertyChanged
{
    private readonly AppSettings settings;
    private readonly IAppSettingsStore store;
    private readonly RelayCommand previousCommand;
    private readonly RelayCommand nextCommand;
    private int stepIndex;

    public TutorialViewModel(AppSettings settings, IAppSettingsStore store, ILocalizationService localization)
    {
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        this.store = store ?? throw new ArgumentNullException(nameof(store));
        ArgumentNullException.ThrowIfNull(localization);
        Steps = Enumerable.Range(1, 5)
            .Select(index => new TutorialStep(
                localization.Get($"Tutorial.Step{index}.Title"),
                localization.Get($"Tutorial.Step{index}.Body")))
            .ToArray();
        previousCommand = new RelayCommand(_ => StepIndex--, _ => StepIndex > 0);
        nextCommand = new RelayCommand(_ => StepIndex++, _ => StepIndex < Steps.Count - 1);
        SkipCommand = new RelayCommand(_ => _ = CompleteAsync(skipped: true));
        FinishCommand = new RelayCommand(_ => _ = CompleteAsync(skipped: false), _ => StepIndex == Steps.Count - 1);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler? RequestClose;
    public IReadOnlyList<TutorialStep> Steps { get; }
    public ICommand PreviousCommand => previousCommand;
    public ICommand NextCommand => nextCommand;
    public ICommand SkipCommand { get; }
    public ICommand FinishCommand { get; }
    public bool CloseRequested { get; private set; }

    public int StepIndex
    {
        get => stepIndex;
        private set
        {
            var bounded = Math.Clamp(value, 0, Steps.Count - 1);
            if (stepIndex == bounded) return;
            stepIndex = bounded;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CurrentStep));
            OnPropertyChanged(nameof(StepNumber));
            previousCommand.RaiseCanExecuteChanged();
            nextCommand.RaiseCanExecuteChanged();
            (FinishCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }

    public TutorialStep CurrentStep => Steps[StepIndex];
    public string StepNumber => $"{StepIndex + 1} / {Steps.Count}";

    public async Task CompleteAsync(bool skipped)
    {
        if (CloseRequested) return;
        await store.SaveAsync(settings with { TutorialCompleted = true });
        CloseRequested = true;
        OnPropertyChanged(nameof(CloseRequested));
        RequestClose?.Invoke(this, EventArgs.Empty);
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
