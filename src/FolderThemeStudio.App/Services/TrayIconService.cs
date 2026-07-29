using System.Drawing;
using Forms = System.Windows.Forms;

namespace FolderThemeStudio.App.Services;

public sealed class TrayIconService : IDisposable
{
    private readonly Forms.NotifyIcon icon;
    private readonly Icon? ownedIcon;

    public TrayIconService()
    {
        var executable = Environment.ProcessPath;
        ownedIcon = executable is null ? null : Icon.ExtractAssociatedIcon(executable);
        var menu = new Forms.ContextMenuStrip();
        menu.Items.Add("打开主窗口", null, (_, _) => OpenRequested?.Invoke(this, EventArgs.Empty));
        menu.Items.Add("暂停 / 恢复监控", null, (_, _) => ToggleMonitoringRequested?.Invoke(this, EventArgs.Empty));
        menu.Items.Add(new Forms.ToolStripSeparator());
        menu.Items.Add("退出", null, (_, _) => ExitRequested?.Invoke(this, EventArgs.Empty));
        icon = new Forms.NotifyIcon
        {
            Text = "文件夹样式",
            Icon = ownedIcon ?? SystemIcons.Application,
            ContextMenuStrip = menu,
            Visible = true,
        };
        icon.DoubleClick += (_, _) => OpenRequested?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? OpenRequested;
    public event EventHandler? ToggleMonitoringRequested;
    public event EventHandler? ExitRequested;

    public void ShowBackgroundNotice() => icon.ShowBalloonTip(
        2500,
        "文件夹样式仍在运行",
        "递归监控已转入后台，可从托盘图标重新打开。",
        Forms.ToolTipIcon.Info);

    public void Dispose()
    {
        icon.Visible = false;
        icon.ContextMenuStrip?.Dispose();
        icon.Dispose();
        ownedIcon?.Dispose();
    }
}
