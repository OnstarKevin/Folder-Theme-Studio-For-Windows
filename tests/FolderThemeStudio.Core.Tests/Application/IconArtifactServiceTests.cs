using System.IO;
using FolderThemeStudio.Core.Application;
using FolderThemeStudio.Core.Rendering;
using FolderThemeStudio.Core.Tests.Rendering;
using FolderThemeStudio.Core.Tests.TestSupport;
using FolderThemeStudio.Core.Themes;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Application;

public sealed class IconArtifactServiceTests
{
    [Fact]
    public async Task CreateAndVerifyAsync_ImportedImage_WritesEveryRequiredFrame()
    {
        using var temp = new TemporaryDirectory();
        var source = TestImageFiles.Write("wide.png", 80, 40, temp.Path);
        var imported = await new ImportedImageService(Path.Combine(temp.Path, "imports"))
            .ImportAsync(source, CancellationToken.None);
        var service = new DefaultIconArtifactService(Path.Combine(temp.Path, "icons"));

        var result = await service.CreateAndVerifyAsync(
            IconSource.ImportedImage(imported.AssetPath!),
            Guid.Parse("55555555-5555-5555-5555-555555555555"),
            CancellationToken.None);

        Assert.True(result.IsSuccess, result.Error);
        Assert.Equal(FolderIconRenderer.RequiredSizes, IcoTestReader.ReadSizes(result.Path!));
    }

    [Fact]
    public async Task CreateAndVerifyAsync_BuiltInTheme_StillWritesEveryRequiredFrame()
    {
        using var temp = new TemporaryDirectory();
        var service = new DefaultIconArtifactService(temp.Path);

        var result = await service.CreateAndVerifyAsync(
            IconSource.BuiltIn(FolderTheme.IceBlue),
            Guid.Parse("66666666-6666-6666-6666-666666666666"),
            CancellationToken.None);

        Assert.True(result.IsSuccess, result.Error);
        Assert.Equal(FolderIconRenderer.RequiredSizes, IcoTestReader.ReadSizes(result.Path!));
    }
}
