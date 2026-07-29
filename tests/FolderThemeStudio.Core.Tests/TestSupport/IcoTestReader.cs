using System.IO;

namespace FolderThemeStudio.Core.Tests.TestSupport;

internal static class IcoTestReader
{
    internal static IReadOnlyList<int> ReadSizes(string icoPath)
    {
        using var stream = File.OpenRead(icoPath);
        using var reader = new BinaryReader(stream);
        AssertHeader(reader);
        var count = reader.ReadUInt16();
        var sizes = new List<int>(count);
        for (var index = 0; index < count; index++)
        {
            var width = reader.ReadByte();
            var height = reader.ReadByte();
            if (height != width) throw new InvalidDataException("ICO frame is not square.");
            sizes.Add(width == 0 ? 256 : width);
            reader.ReadBytes(14);
        }
        return sizes;
    }

    private static void AssertHeader(BinaryReader reader)
    {
        if (reader.ReadUInt16() != 0 || reader.ReadUInt16() != 1)
        {
            throw new InvalidDataException("Not an ICO file.");
        }
    }
}
