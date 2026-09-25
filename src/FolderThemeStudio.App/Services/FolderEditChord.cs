using System.Windows;
using System.Windows.Input;
using Point = System.Windows.Point;

namespace FolderThemeStudio.App.Services;

public enum FolderMouseButton { Right, Middle }

public sealed record FolderEditChord(ModifierKeys Modifiers, FolderMouseButton Button)
{
    private const ModifierKeys Allowed = ModifierKeys.Alt | ModifierKeys.Control | ModifierKeys.Shift | ModifierKeys.Windows;
    public static FolderEditChord Default { get; } = new(ModifierKeys.Alt, FolderMouseButton.Right);

    public static bool TryNormalize(FolderEditChord? chord, out FolderEditChord normalized)
    {
        normalized = Default;
        if (chord is null || chord.Modifiers == ModifierKeys.None || (chord.Modifiers & ~Allowed) != 0 ||
            chord.Button is not (FolderMouseButton.Right or FolderMouseButton.Middle)) return false;
        normalized = chord;
        return true;
    }

    public override string ToString()
    {
        var parts = new List<string>();
        if (Modifiers.HasFlag(ModifierKeys.Control)) parts.Add("Ctrl");
        if (Modifiers.HasFlag(ModifierKeys.Alt)) parts.Add("Alt");
        if (Modifiers.HasFlag(ModifierKeys.Shift)) parts.Add("Shift");
        if (Modifiers.HasFlag(ModifierKeys.Windows)) parts.Add("Win");
        parts.Add(Button == FolderMouseButton.Right ? "Right Click" : "Middle Click");
        return string.Join(" + ", parts);
    }
}

public sealed class FolderQuickEditDecision(FolderEditChord chord)
{
    private FolderMouseButton? consumedButton;
    private FolderEditChord chord = chord;

    public void UpdateChord(FolderEditChord value) => chord = FolderEditChord.TryNormalize(value, out var normalized)
        ? normalized : FolderEditChord.Default;

    public string? OnMouseDown(FolderMouseButton button, ModifierKeys modifiers, Point point, ExplorerHoverTarget? target)
    {
        if (button != chord.Button || modifiers != chord.Modifiers || target is null || !target.ItemBounds.Contains(point)) return null;
        consumedButton = button;
        return target.FolderPath;
    }

    public bool OnMouseUp(FolderMouseButton button)
    {
        if (consumedButton != button) return false;
        consumedButton = null;
        return true;
    }
}
