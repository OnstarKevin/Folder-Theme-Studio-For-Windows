using FolderThemeStudio.App;
using FolderThemeStudio.App.Controls;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class RoundedWindowTests
{
    [Fact]
    public void EveryApplicationWindow_UsesSharedRoundedWindowBase()
    {
        Assert.Equal(typeof(RoundedWindow), typeof(MainWindow).BaseType);
        Assert.Equal(typeof(RoundedWindow), typeof(SettingsWindow).BaseType);
        Assert.Equal(typeof(RoundedWindow), typeof(TutorialWindow).BaseType);
    }
}
