using FolderThemeStudio.Core.Rendering;
using Xunit;

namespace FolderThemeStudio.Core.Tests.Rendering;

public sealed class IcoEncoderTests
{
    [Fact]
    public void Encode_WritesOneDirectoryEntryPerRequiredSize()
    {
        var frames = FolderIconRenderer.RequiredSizes.ToDictionary(size => size, TestPng.Create);

        var ico = IcoEncoder.Encode(frames);

        Assert.Equal(FolderIconRenderer.RequiredSizes.Length, BitConverter.ToUInt16(ico, 4));
    }

    [Fact]
    public void Encode_DirectoryEntriesPointToPngPayloads()
    {
        var frames = FolderIconRenderer.RequiredSizes.ToDictionary(size => size, TestPng.Create);

        var ico = IcoEncoder.Encode(frames);

        Assert.Equal((ushort)0, BitConverter.ToUInt16(ico, 0));
        Assert.Equal((ushort)1, BitConverter.ToUInt16(ico, 2));
        foreach (var entryIndex in Enumerable.Range(0, FolderIconRenderer.RequiredSizes.Length))
        {
            var entryOffset = 6 + (entryIndex * 16);
            var payloadOffset = BitConverter.ToInt32(ico, entryOffset + 12);
            var payloadLength = BitConverter.ToInt32(ico, entryOffset + 8);

            Assert.InRange(payloadOffset, 6 + FolderIconRenderer.RequiredSizes.Length * 16, ico.Length - 8);
            Assert.InRange(payloadLength, 8, ico.Length - payloadOffset);
            Assert.Equal(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }, ico[payloadOffset..(payloadOffset + 8)]);
        }
    }

    [Fact]
    public void Encode_MissingRequiredFrame_ThrowsArgumentException()
    {
        var frames = FolderIconRenderer.RequiredSizes
            .Where(size => size != 256)
            .ToDictionary(size => size, TestPng.Create);

        Assert.Throws<ArgumentException>(() => IcoEncoder.Encode(frames));
    }

    [Fact]
    public void Encode_UnexpectedFrameSize_ThrowsArgumentException()
    {
        var frames = FolderIconRenderer.RequiredSizes.ToDictionary(size => size, TestPng.Create);
        frames.Add(40, TestPng.Create(40));

        Assert.Throws<ArgumentException>(() => IcoEncoder.Encode(frames));
    }
}
