using System.Windows.Media;
using FolderThemeStudio.Core.Rendering;
using FolderThemeStudio.Core.Themes;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Rendering;

public sealed class FolderIconRendererTests
{
    public static IEnumerable<object[]> Sizes() =>
        FolderIconRenderer.RequiredSizes.Select(size => new object[] { size });

    [Theory]
    [MemberData(nameof(Sizes))]
    public void Render_ReturnsRequestedSquareBitmap(int size)
    {
        var bitmap = FolderIconRenderer.Render(FolderTheme.IceBlue, size);

        Assert.Equal(size, bitmap.PixelWidth);
        Assert.Equal(size, bitmap.PixelHeight);
        Assert.True(bitmap.IsFrozen);
    }

    [Theory]
    [MemberData(nameof(Sizes))]
    public void Render_VisiblePixelsHaveATransparentInsetOnEveryEdge(int size)
    {
        var bitmap = FolderIconRenderer.Render(FolderTheme.IceBlue, size);
        var pixels = new byte[size * size * 4];
        bitmap.CopyPixels(pixels, size * 4, 0);

        var opaquePixelIndexes = Enumerable.Range(0, size * size)
            .Where(index => pixels[index * 4 + 3] != 0)
            .ToArray();

        Assert.NotEmpty(opaquePixelIndexes);
        var xs = opaquePixelIndexes.Select(index => index % size).ToArray();
        var ys = opaquePixelIndexes.Select(index => index / size).ToArray();

        Assert.InRange(xs.Min(), 1, size - 2);
        Assert.InRange(xs.Max(), 1, size - 2);
        Assert.InRange(ys.Min(), 1, size - 2);
        Assert.InRange(ys.Max(), 1, size - 2);
    }

    [Fact]
    public void Render_UnsupportedSize_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            FolderIconRenderer.Render(FolderTheme.IceBlue, 40));
    }
}
