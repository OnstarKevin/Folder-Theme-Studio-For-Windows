using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace FolderThemeStudio.App.ViewModels;

public sealed class ColorPickerViewModel : INotifyPropertyChanged
{
    private double hue;
    private double saturation;
    private double value;
    private string hex = "#000000";
    private bool isValid;
    private bool updating;
    private System.Windows.Media.Brush selectedBrush = System.Windows.Media.Brushes.Black;
    private System.Windows.Media.Brush hueBrush = System.Windows.Media.Brushes.Red;

    public ColorPickerViewModel(string initialHex)
    {
        if (!TrySetHex(initialHex))
        {
            throw new ArgumentException("Initial color must use RGB hexadecimal format.", nameof(initialHex));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public double Hue
    {
        get => hue;
        set
        {
            var normalized = ((value % 360) + 360) % 360;
            if (SetField(ref hue, normalized) && !updating) UpdateHexFromHsv();
        }
    }

    public double Saturation
    {
        get => saturation;
        set
        {
            if (SetField(ref saturation, Math.Clamp(value, 0, 1)) && !updating) UpdateHexFromHsv();
        }
    }

    public double Value
    {
        get => value;
        set
        {
            if (SetField(ref this.value, Math.Clamp(value, 0, 1)) && !updating) UpdateHexFromHsv();
        }
    }

    public string Hex
    {
        get => hex;
        set
        {
            if (TryNormalizeHex(value, out var normalized))
            {
                SetFromHex(normalized);
                return;
            }

            SetField(ref hex, value ?? string.Empty);
            SetField(ref isValid, false, nameof(IsValid));
        }
    }

    public bool IsValid => isValid;
    public System.Windows.Media.Brush SelectedBrush => selectedBrush;
    public System.Windows.Media.Brush HueBrush => hueBrush;

    public bool TrySetHex(string value)
    {
        if (!TryNormalizeHex(value, out var normalized))
        {
            return false;
        }

        SetFromHex(normalized);
        return true;
    }

    public void SetPalettePoint(double x, double y, double width, double height)
    {
        var selected = VisualColorMath.FromPalettePoint(x, y, width, height);
        Saturation = selected.Saturation;
        Value = selected.Value;
    }

    public void SetHuePoint(double y, double height) =>
        Hue = VisualColorMath.HueFromPoint(y, height);

    private void SetFromHex(string normalized)
    {
        var red = byte.Parse(normalized.AsSpan(1, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        var green = byte.Parse(normalized.AsSpan(3, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        var blue = byte.Parse(normalized.AsSpan(5, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        RgbToHsv(red, green, blue, out var nextHue, out var nextSaturation, out var nextValue);

        updating = true;
        try
        {
            SetField(ref hue, nextHue, nameof(Hue));
            SetField(ref saturation, nextSaturation, nameof(Saturation));
            SetField(ref value, nextValue, nameof(Value));
            SetField(ref hex, normalized, nameof(Hex));
            SetField(ref isValid, true, nameof(IsValid));
            SetBrush(red, green, blue);
            SetHueBrush(nextHue);
        }
        finally
        {
            updating = false;
        }
    }

    private void UpdateHexFromHsv()
    {
        HsvToRgb(Hue, Saturation, Value, out var red, out var green, out var blue);
        var normalized = $"#{red:X2}{green:X2}{blue:X2}";
        SetField(ref hex, normalized, nameof(Hex));
        SetField(ref isValid, true, nameof(IsValid));
        SetBrush(red, green, blue);
        SetHueBrush(Hue);
    }

    private void SetHueBrush(double selectedHue)
    {
        HsvToRgb(selectedHue, 1, 1, out var red, out var green, out var blue);
        var brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(red, green, blue));
        brush.Freeze();
        hueBrush = brush;
        OnPropertyChanged(nameof(HueBrush));
    }

    private void SetBrush(byte red, byte green, byte blue)
    {
        var brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(red, green, blue));
        brush.Freeze();
        selectedBrush = brush;
        OnPropertyChanged(nameof(SelectedBrush));
    }

    private static bool TryNormalizeHex(string? input, out string normalized)
    {
        var value = (input ?? string.Empty).Trim();
        if (!value.StartsWith('#')) value = "#" + value;
        if (value.Length == 4)
        {
            value = $"#{value[1]}{value[1]}{value[2]}{value[2]}{value[3]}{value[3]}";
        }

        normalized = value.ToUpperInvariant();
        return normalized.Length == 7 && normalized.AsSpan(1).ToString().All(Uri.IsHexDigit);
    }

    private static void RgbToHsv(byte red, byte green, byte blue, out double hue, out double saturation, out double value)
    {
        var r = red / 255d;
        var g = green / 255d;
        var b = blue / 255d;
        var max = Math.Max(r, Math.Max(g, b));
        var min = Math.Min(r, Math.Min(g, b));
        var delta = max - min;
        value = max;
        saturation = max == 0 ? 0 : delta / max;
        if (delta == 0) hue = 0;
        else if (max == r) hue = 60 * (((g - b) / delta) % 6);
        else if (max == g) hue = 60 * (((b - r) / delta) + 2);
        else hue = 60 * (((r - g) / delta) + 4);
        if (hue < 0) hue += 360;
    }

    private static void HsvToRgb(double hue, double saturation, double value, out byte red, out byte green, out byte blue)
    {
        var chroma = value * saturation;
        var x = chroma * (1 - Math.Abs(((hue / 60) % 2) - 1));
        var m = value - chroma;
        var sector = (int)Math.Floor(hue / 60) % 6;
        var (r, g, b) = sector switch
        {
            0 => (chroma, x, 0d),
            1 => (x, chroma, 0d),
            2 => (0d, chroma, x),
            3 => (0d, x, chroma),
            4 => (x, 0d, chroma),
            _ => (chroma, 0d, x),
        };
        red = (byte)Math.Round((r + m) * 255);
        green = (byte)Math.Round((g + m) * 255);
        blue = (byte)Math.Round((b + m) * 255);
    }

    private bool SetField<T>(ref T field, T next, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, next)) return false;
        field = next;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
