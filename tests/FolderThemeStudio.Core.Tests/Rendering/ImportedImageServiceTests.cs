using System.IO;
using FolderThemeStudio.Core.Rendering;
using FolderThemeStudio.Core.Tests.TestSupport;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Rendering;

public sealed class ImportedImageServiceTests
{
    [Theory]
    [InlineData("sample.png")]
    [InlineData("sample.jpg")]
    [InlineData("sample.jpeg")]
    [InlineData("sample.bmp")]
    public async Task ImportAsync_SupportedImage_NormalizesToPersistentPng(string fileName)
    {
        using var temp = new TemporaryDirectory();
        var source = TestImageFiles.Write(fileName, 80, 40, temp.Path);

        var result = await new ImportedImageService(Path.Combine(temp.Path, "imports"))
            .ImportAsync(source, CancellationToken.None);

        Assert.True(result.Success, result.Error);
        Assert.NotNull(result.Preview);
        Assert.EndsWith(".png", result.AssetPath, StringComparison.OrdinalIgnoreCase);
        Assert.True(File.Exists(result.AssetPath));
        Assert.StartsWith(Path.Combine(temp.Path, "imports"), result.AssetPath, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ImportAsync_GifContentIsRejected()
    {
        using var temp = new TemporaryDirectory();
        var path = Path.Combine(temp.Path, "animated.gif");
        await File.WriteAllBytesAsync(path, "GIF89a"u8.ToArray());

        var result = await new ImportedImageService(Path.Combine(temp.Path, "imports"))
            .ImportAsync(path, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Null(result.AssetPath);
    }

    [Fact]
    public async Task ImportAsync_ExtensionDoesNotMatchContent_IsRejected()
    {
        using var temp = new TemporaryDirectory();
        var png = TestImageFiles.Write("actual.png", 16, 16, temp.Path);
        var disguised = Path.Combine(temp.Path, "disguised.jpg");
        File.Copy(png, disguised);

        var result = await new ImportedImageService(Path.Combine(temp.Path, "imports"))
            .ImportAsync(disguised, CancellationToken.None);

        Assert.False(result.Success);
    }

    [Fact]
    public async Task ImportAsync_CorruptSupportedFile_IsRejected()
    {
        using var temp = new TemporaryDirectory();
        var path = Path.Combine(temp.Path, "broken.png");
        await File.WriteAllBytesAsync(path, [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00]);

        var result = await new ImportedImageService(Path.Combine(temp.Path, "imports"))
            .ImportAsync(path, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Null(result.Preview);
    }
}
