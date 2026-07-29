using System.ComponentModel;
using FolderThemeStudio.App.Settings;

namespace FolderThemeStudio.App.ViewModels;

public sealed record PaletteEditorRole(string DisplayName, ColorPickerViewModel Picker);

public sealed class PaletteEditorViewModel : INotifyPropertyChanged
{
    private PaletteEditorRole selectedRole;
    public PaletteEditorViewModel(FolderPalette initial)
    {
        GradientStart = new(initial.GradientStart);
        GradientEnd = new(initial.GradientEnd);
        Stroke = new(initial.Stroke);
        Glow = new(initial.Glow);
        Roles =
        [
            new("渐变起始色", GradientStart),
            new("渐变结束色", GradientEnd),
            new("边框颜色", Stroke),
            new("发光颜色", Glow)
        ];
        selectedRole = Roles[0];
        foreach (var role in Roles) role.Picker.PropertyChanged += PickerChanged;
    }

    public ColorPickerViewModel GradientStart { get; }
    public ColorPickerViewModel GradientEnd { get; }
    public ColorPickerViewModel Stroke { get; }
    public ColorPickerViewModel Glow { get; }
    public IReadOnlyList<PaletteEditorRole> Roles { get; }
    public PaletteEditorRole SelectedRole
    {
        get => selectedRole;
        set
        {
            if (Equals(selectedRole, value)) return;
            selectedRole = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedRole)));
        }
    }
    public FolderPalette Palette => new(GradientStart.Hex, GradientEnd.Hex, Stroke.Hex, Glow.Hex);
    public event EventHandler? PaletteChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    public void Apply(FolderPalette palette)
    {
        GradientStart.TrySetHex(palette.GradientStart);
        GradientEnd.TrySetHex(palette.GradientEnd);
        Stroke.TrySetHex(palette.Stroke);
        Glow.TrySetHex(palette.Glow);
    }

    private void PickerChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ColorPickerViewModel.Hex) &&
            GradientStart.IsValid && GradientEnd.IsValid && Stroke.IsValid && Glow.IsValid)
        {
            PaletteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
