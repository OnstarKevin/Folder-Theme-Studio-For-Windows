using System.IO;
using System.Security.Cryptography;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FolderThemeStudio.Core.Rendering;

public sealed record ImportedImageResult(
    bool Success,
    string? AssetPath,
    BitmapSource? Preview,
    string? Error)
{
    public static ImportedImageResult Succeeded(string assetPath, BitmapSource preview) =>
        new(true, assetPath, preview, null);

    public static ImportedImageResult Failure(string error) =>
        new(false, null, null, error);
}

public interface IImportedImageService
{
    Task<ImportedImageResult> ImportAsync(string sourcePath, CancellationToken token);
}

public sealed class ImportedImageService : IImportedImageService
{
    private static readonly byte[] PngSignature = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];
    private static readonly byte[] IcoSignature = [0x00, 0x00, 0x01, 0x00];
    private readonly string importsRoot;

    public ImportedImageService()
        : this(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FolderThemeStudio",
            "imports"))
    {
    }

    public ImportedImageService(string importsRoot)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(importsRoot);
        this.importsRoot = Path.GetFullPath(importsRoot);
    }

    public async Task<ImportedImageResult> ImportAsync(string sourcePath, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(sourcePath))
        {
            return ImportedImageResult.Failure("An image path is required.");
        }

        try
        {
            token.ThrowIfCancellationRequested();
            var fullSourcePath = Path.GetFullPath(sourcePath);
            var bytes = await File.ReadAllBytesAsync(fullSourcePath, token).ConfigureAwait(false);
            var detected = DetectFormat(bytes);
            var expected = FormatForExtension(Path.GetExtension(fullSourcePath));
            if (detected is null || expected is null || detected != expected)
            {
                return ImportedImageResult.Failure("Choose a valid PNG, JPEG, BMP, or ICO image whose extension matches its contents.");
            }

            var normalized = Decode(bytes, detected);
            var pngBytes = EncodePng(normalized);
            var hash = Convert.ToHexString(SHA256.HashData(pngBytes)).ToLowerInvariant();
            Directory.CreateDirectory(importsRoot);
            var destination = Path.Combine(importsRoot, hash + ".png");
            if (!File.Exists(destination))
            {
                var temporary = Path.Combine(importsRoot, $".{hash}.{Guid.NewGuid():N}.tmp");
                try
                {
                    await File.WriteAllBytesAsync(temporary, pngBytes, token).ConfigureAwait(false);
                    try
                    {
                        File.Move(temporary, destination);
                    }
                    catch (IOException) when (File.Exists(destination))
                    {
                        // Another import persisted the identical content first.
                    }
                }
                finally
                {
                    if (File.Exists(temporary)) File.Delete(temporary);
                }
            }

            var preview = Decode(
                await File.ReadAllBytesAsync(destination, token).ConfigureAwait(false),
                "png");
            return ImportedImageResult.Succeeded(destination, preview);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception exception) when (exception is IOException
            or UnauthorizedAccessException
            or System.Security.SecurityException
            or NotSupportedException
            or ArgumentException
            or InvalidOperationException
            or FileFormatException)
        {
            return ImportedImageResult.Failure(exception.Message);
        }
    }

    private static string? DetectFormat(ReadOnlySpan<byte> bytes)
    {
        if (bytes.StartsWith(PngSignature)) return "png";
        if (bytes.StartsWith(IcoSignature)) return "ico";
        if (bytes.Length >= 3 && bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF) return "jpeg";
        if (bytes.Length >= 2 && bytes[0] == 0x42 && bytes[1] == 0x4D) return "bmp";
        return null;
    }

    private static string? FormatForExtension(string extension) => extension.ToLowerInvariant() switch
    {
        ".png" => "png",
        ".jpg" or ".jpeg" => "jpeg",
        ".bmp" => "bmp",
        ".ico" => "ico",
        _ => null
    };

    private static BitmapSource Decode(byte[] bytes, string format)
    {
        using var stream = new MemoryStream(bytes, writable: false);
        var decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
        if (decoder.Frames.Count == 0) throw new FileFormatException("The image has no decodable frame.");
        var source = format == "ico"
            ? decoder.Frames
                .OrderByDescending(frame => checked((long)frame.PixelWidth * frame.PixelHeight))
                .ThenByDescending(frame => frame.Format.BitsPerPixel)
                .First()
            : decoder.Frames[0];
        var converted = new FormatConvertedBitmap(source, PixelFormats.Pbgra32, null, 0);
        converted.Freeze();
        return converted;
    }

    private static byte[] EncodePng(BitmapSource source)
    {
        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(source));
        using var stream = new MemoryStream();
        encoder.Save(stream);
        return stream.ToArray();
    }
}
