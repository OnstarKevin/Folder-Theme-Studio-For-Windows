using System.Text;
using FolderThemeStudio.Core.SystemIntegration;
using Xunit;

namespace FolderThemeStudio.Core.Tests.SystemIntegration;

public sealed class DesktopIniDocumentTests
{
    [Fact]
    public void Merge_AbsentDocumentCreatesShellClassInfo()
    {
        var result = DesktopIniDocument.Merge(null, @"C:\Icons\new.ico");

        Assert.True(result.Success, result.Error);
        Assert.Equal(
            "[.ShellClassInfo]\r\nIconFile=C:\\Icons\\new.ico\r\nIconIndex=0\r\n",
            Encoding.UTF8.GetString(result.Bytes!));
    }

    [Fact]
    public void Merge_PreservesUnrelatedKeysAndReplacesIconValues()
    {
        var original = WithUnicodeBom(
            "[.ShellClassInfo]\r\n;keep\r\nLocalizedResourceName=@shell32.dll,-1\r\nIconResource=old.ico,2\r\nIconFile=old.ico\r\nIconIndex=2\r\n[ViewState]\r\nMode=\r\n");

        var result = DesktopIniDocument.Merge(original, @"C:\Icons\new.ico");

        Assert.True(result.Success, result.Error);
        Assert.True(result.Bytes!.AsSpan().StartsWith(Encoding.Unicode.GetPreamble()));
        var text = Encoding.Unicode.GetString(result.Bytes.AsSpan(2));
        Assert.Contains(";keep\r\nLocalizedResourceName=@shell32.dll,-1", text);
        Assert.Contains("IconFile=C:\\Icons\\new.ico\r\nIconIndex=0", text);
        Assert.Contains("[ViewState]\r\nMode=", text);
        Assert.DoesNotContain("IconResource=", text, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("old.ico", text);
    }

    [Fact]
    public void Merge_PreservesUtf8BomAndLfLineEndings()
    {
        var original = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes("[.ShellClassInfo]\nIconFile=old.ico\nIconIndex=4\n")).ToArray();

        var result = DesktopIniDocument.Merge(original, @"D:\folder.ico");

        Assert.True(result.Success, result.Error);
        Assert.True(result.Bytes!.AsSpan().StartsWith(Encoding.UTF8.GetPreamble()));
        var text = Encoding.UTF8.GetString(result.Bytes.AsSpan(3));
        Assert.DoesNotContain("\r\n", text);
        Assert.Contains("IconFile=D:\\folder.ico\nIconIndex=0\n", text);
    }

    [Fact]
    public void Merge_AppendsShellSectionWithoutChangingExistingSections()
    {
        var original = Encoding.UTF8.GetBytes(";header\r\n[ViewState]\r\nMode=Tiles\r\n");

        var result = DesktopIniDocument.Merge(original, @"C:\new.ico");

        var text = Encoding.UTF8.GetString(result.Bytes!);
        Assert.StartsWith(";header\r\n[ViewState]\r\nMode=Tiles\r\n", text);
        Assert.EndsWith("[.ShellClassInfo]\r\nIconFile=C:\\new.ico\r\nIconIndex=0\r\n", text);
    }

    [Fact]
    public void Merge_IsIdempotent()
    {
        var first = DesktopIniDocument.Merge(null, @"C:\same.ico");
        var second = DesktopIniDocument.Merge(first.Bytes, @"C:\same.ico");

        Assert.True(second.Success);
        Assert.Equal(first.Bytes, second.Bytes);
    }

    [Theory]
    [InlineData("[broken\r\nvalue=1\r\n")]
    [InlineData("name=value\r\n")]
    [InlineData("[.ShellClassInfo]\r\ninvalid line\r\n")]
    public void Merge_UnsafeSyntaxFailsWithoutOutput(string text)
    {
        var result = DesktopIniDocument.Merge(Encoding.UTF8.GetBytes(text), @"C:\new.ico");

        Assert.False(result.Success);
        Assert.Null(result.Bytes);
        Assert.NotEmpty(result.Error!);
    }

    [Fact]
    public void Merge_NulInputFailsWithoutOutput()
    {
        var result = DesktopIniDocument.Merge([0, 1, 2], @"C:\new.ico");

        Assert.False(result.Success);
        Assert.Null(result.Bytes);
    }

    private static byte[] WithUnicodeBom(string value) =>
        Encoding.Unicode.GetPreamble().Concat(Encoding.Unicode.GetBytes(value)).ToArray();
}
