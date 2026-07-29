using System.IO;
using System.Windows.Media.Imaging;

namespace FolderThemeStudio.Core.Rendering;

public static class IcoEncoder
{
    private static readonly byte[] PngSignature = [137, 80, 78, 71, 13, 10, 26, 10];

    public static byte[] Encode(IReadOnlyDictionary<int, byte[]> pngFrames)
    {
        ArgumentNullException.ThrowIfNull(pngFrames);
        ValidateFrameSet(pngFrames);

        var orderedFrames = FolderIconRenderer.RequiredSizes
            .Select(size => new KeyValuePair<int, byte[]>(size, pngFrames[size]))
            .ToArray();
        var payloadOffset = checked(6 + (orderedFrames.Length * 16));

        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
        writer.Write((ushort)0);
        writer.Write((ushort)1);
        writer.Write((ushort)orderedFrames.Length);

        foreach (var frame in orderedFrames)
        {
            writer.Write((byte)(frame.Key == 256 ? 0 : frame.Key));
            writer.Write((byte)(frame.Key == 256 ? 0 : frame.Key));
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write((ushort)1);
            writer.Write((ushort)32);
            writer.Write(frame.Value.Length);
            writer.Write(payloadOffset);
            payloadOffset = checked(payloadOffset + frame.Value.Length);
        }

        foreach (var frame in orderedFrames)
        {
            writer.Write(frame.Value);
        }

        writer.Flush();
        return stream.ToArray();
    }

    private static void ValidateFrameSet(IReadOnlyDictionary<int, byte[]> pngFrames)
    {
        if (pngFrames.Count != FolderIconRenderer.RequiredSizes.Length ||
            pngFrames.Keys.Any(size => !FolderIconRenderer.RequiredSizes.Contains(size)))
        {
            throw new ArgumentException("Frames must contain exactly one PNG for every required size.", nameof(pngFrames));
        }

        foreach (var size in FolderIconRenderer.RequiredSizes)
        {
            if (!pngFrames.TryGetValue(size, out var frame))
            {
                throw new ArgumentException("Frames must contain exactly one PNG for every required size.", nameof(pngFrames));
            }

            ValidatePngFrame(size, frame);
        }
    }

    private static void ValidatePngFrame(int size, byte[] frame)
    {
        if (frame is null || frame.Length < PngSignature.Length || !frame.AsSpan(0, PngSignature.Length).SequenceEqual(PngSignature))
        {
            throw new ArgumentException("Every frame must be a valid PNG payload.", nameof(frame));
        }

        try
        {
            using var stream = new MemoryStream(frame, writable: false);
            var decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            if (decoder.Frames.Count != 1 || decoder.Frames[0].PixelWidth != size || decoder.Frames[0].PixelHeight != size)
            {
                throw new ArgumentException("PNG dimensions must match the frame size.", nameof(frame));
            }
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new ArgumentException("Every frame must be a valid PNG payload.", nameof(frame), exception);
        }
    }
}
