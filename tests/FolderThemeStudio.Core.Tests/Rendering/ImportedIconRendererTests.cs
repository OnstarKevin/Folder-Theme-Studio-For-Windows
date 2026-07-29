using System.IO;
using FolderThemeStudio.Core.Rendering;
using FolderThemeStudio.Core.Tests.TestSupport;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Rendering;

public sealed class ImportedIconRendererTests
{
    [Fact]
    public async Task Render_WideImage_IsCenteredWithTransparentPaddingAndNoCropping()
    {
        using var temp = new TemporaryDirectory();
        var source = TestImageFiles.Write("wide.png", 80, 40, temp.Path);
        var imported = await new ImportedImageService(Path.Combine(temp.Path, "imports"))
            .ImportAsync(source, CancellationToken.None);

        var bitmap = ImportedIconRenderer.Render(imported.AssetPath!, 64);

        Assert.Equal(64, bitmap.PixelWidth);
        Assert.Equal(64, bitmap.PixelHeight);
        Assert.Equal(0, TestImageFiles.AlphaAt(bitmap, 32, 4));
        Assert.Equal(255, TestImageFiles.AlphaAt(bitmap, 32, 32));
        Assert.Equal(255, TestImageFiles.AlphaAt(bitmap, 1, 32));
        Assert.Equal(255, TestImageFiles.AlphaAt(bitmap, 62, 32));
    }
}
