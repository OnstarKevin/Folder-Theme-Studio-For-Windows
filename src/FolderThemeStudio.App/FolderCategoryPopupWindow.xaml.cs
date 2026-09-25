using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using FolderThemeStudio.App.Services;
using Brush = System.Windows.Media.Brush;
using Point = System.Windows.Point;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace FolderThemeStudio.App;

public partial class FolderCategoryPopupWindow : Window, IFolderHoverCardPresenter
{
    private const uint SwpNoSize = 0x0001;
    private const uint SwpNoActivate = 0x0010;
    private const uint SwpShowWindow = 0x0040;
    private const uint MonitorDefaultToNearest = 2;
    private static readonly nint HwndTopmostHandle = new(-1);
    private nint handle;

    public FolderCategoryPopupWindow()
    {
        InitializeComponent();
        SourceInitialized += (_, _) => ApplyClickThroughStyle();
    }

    public void Show(FolderCategoryEntry entry, Rect itemBounds)
    {
        CategoryTitle.Text = entry.Category;
        CategoryNote.Text = entry.Note;
        CategoryNote.Visibility = string.IsNullOrWhiteSpace(entry.Note) ? Visibility.Collapsed : Visibility.Visible;
        CategoryAccent.Background = new LinearGradientBrush(
            (System.Windows.Media.Color)ColorConverter.ConvertFromString(entry.GradientStartHex),
            (System.Windows.Media.Color)ColorConverter.ConvertFromString(entry.GradientEndHex),
            new Point(0, 0), new Point(0, 1));

        if (!IsVisible) base.Show();
        UpdateLayout();
        PositionNear(itemBounds);
    }

    public new void Hide()
    {
        if (IsVisible) base.Hide();
    }

    public void SetCardOpacity(double opacity) => CardPanel.Opacity = Math.Clamp(opacity, 0, 1);

    private void ApplyClickThroughStyle()
    {
        handle = new WindowInteropHelper(this).Handle;
        var style = NativeMethods.GetWindowLongPtr(handle, NativeMethods.GwlExStyle).ToInt64();
        style |= NativeMethods.WsExNoActivate | NativeMethods.WsExToolWindow | NativeMethods.WsExTransparent;
        NativeMethods.SetWindowLongPtr(handle, NativeMethods.GwlExStyle, new nint(style));
    }

    private void PositionNear(Rect bounds)
    {
        if (handle == nint.Zero) handle = new WindowInteropHelper(this).Handle;
        if (!NativeMethods.GetWindowRect(handle, out var windowRect)) return;
        var monitor = NativeMethods.MonitorFromPoint(new NativeMethods.NativePoint((int)bounds.Left, (int)bounds.Top), MonitorDefaultToNearest);
        var info = new NativeMethods.MonitorInfo { Size = Marshal.SizeOf<NativeMethods.MonitorInfo>() };
        if (monitor == nint.Zero || !NativeMethods.GetMonitorInfo(monitor, ref info)) return;

        var width = windowRect.Right - windowRect.Left;
        var height = windowRect.Bottom - windowRect.Top;
        var x = (int)bounds.Right + 18;
        var y = (int)bounds.Top;
        if (x + width > info.Work.Right) x = (int)bounds.Left - width - 18;
        if (y + height > info.Work.Bottom) y = info.Work.Bottom - height;
        x = Math.Clamp(x, info.Work.Left, Math.Max(info.Work.Left, info.Work.Right - width));
        y = Math.Clamp(y, info.Work.Top, Math.Max(info.Work.Top, info.Work.Bottom - height));
        NativeMethods.SetWindowPos(handle, HwndTopmostHandle, x, y, 0, 0, SwpNoSize | SwpNoActivate | SwpShowWindow);
    }

    private static class NativeMethods
    {
        internal const int GwlExStyle = -20;
        internal const long WsExTransparent = 0x00000020L;
        internal const long WsExToolWindow = 0x00000080L;
        internal const long WsExNoActivate = 0x08000000L;

        [StructLayout(LayoutKind.Sequential)]
        internal struct NativePoint
        {
            internal int X;
            internal int Y;
            internal NativePoint(int x, int y) { X = x; Y = y; }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct NativeRect { internal int Left; internal int Top; internal int Right; internal int Bottom; }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct MonitorInfo
        {
            internal int Size;
            internal NativeRect Monitor;
            internal NativeRect Work;
            internal uint Flags;
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtrW", SetLastError = true)]
        internal static extern nint GetWindowLongPtr(nint hwnd, int index);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW", SetLastError = true)]
        internal static extern nint SetWindowLongPtr(nint hwnd, int index, nint value);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowRect(nint hwnd, out NativeRect rect);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern nint MonitorFromPoint(NativePoint point, uint flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetMonitorInfo(nint monitor, ref MonitorInfo info);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetWindowPos(nint hwnd, nint insertAfter, int x, int y, int width, int height, uint flags);
    }
}
