using System.Text;

namespace FolderThemeStudio.Core.SystemIntegration;

internal sealed record DesktopIniMergeResult(bool Success, byte[]? Bytes, string? Error)
{
    internal static DesktopIniMergeResult Succeeded(byte[] bytes) => new(true, bytes, null);
    internal static DesktopIniMergeResult Failure(string error) => new(false, null, error);
}

internal static class DesktopIniDocument
{
    private const string ShellSection = ".ShellClassInfo";

    internal static DesktopIniMergeResult Merge(byte[]? originalBytes, string icoPath)
    {
        if (string.IsNullOrWhiteSpace(icoPath) || icoPath.IndexOfAny(['\r', '\n', '\0']) >= 0)
            return DesktopIniMergeResult.Failure("The icon path is invalid.");

        try
        {
            var source = originalBytes ?? [];
            var decoded = Decode(source);
            if (!decoded.Success) return DesktopIniMergeResult.Failure(decoded.Error!);

            var newline = decoded.Text!.Length == 0 || decoded.Text.Contains("\r\n", StringComparison.Ordinal) ? "\r\n" : "\n";
            var hadTrailingNewline = decoded.Text.Length > 0 &&
                (decoded.Text.EndsWith("\n", StringComparison.Ordinal) || decoded.Text.EndsWith("\r", StringComparison.Ordinal));
            var normalized = decoded.Text.Replace("\r\n", "\n", StringComparison.Ordinal).Replace('\r', '\n');
            var lines = normalized.Length == 0
                ? new List<string>()
                : normalized.Split('\n').ToList();
            if (hadTrailingNewline && lines.Count > 0 && lines[^1].Length == 0) lines.RemoveAt(lines.Count - 1);

            var sectionNames = new string?[lines.Count];
            string? currentSection = null;
            var shellSectionCount = 0;
            for (var index = 0; index < lines.Count; index++)
            {
                var trimmed = lines[index].Trim();
                if (trimmed.Length == 0 || trimmed.StartsWith(';') || trimmed.StartsWith('#'))
                {
                    sectionNames[index] = currentSection;
                    continue;
                }

                if (trimmed.StartsWith('['))
                {
                    if (!trimmed.EndsWith(']') || trimmed.Length < 3 || trimmed[1..^1].Contains('[') || trimmed[1..^1].Contains(']'))
                        return DesktopIniMergeResult.Failure($"Unsafe section syntax on line {index + 1}.");
                    currentSection = trimmed[1..^1].Trim();
                    if (currentSection.Length == 0)
                        return DesktopIniMergeResult.Failure($"Empty section on line {index + 1}.");
                    if (currentSection.Equals(ShellSection, StringComparison.OrdinalIgnoreCase)) shellSectionCount++;
                    if (shellSectionCount > 1)
                        return DesktopIniMergeResult.Failure("Multiple .ShellClassInfo sections are not supported.");
                    sectionNames[index] = currentSection;
                    continue;
                }

                if (currentSection is null || !trimmed.Contains('='))
                    return DesktopIniMergeResult.Failure($"Unsafe key syntax on line {index + 1}.");
                sectionNames[index] = currentSection;
            }

            var output = new List<string>();
            if (shellSectionCount == 0)
            {
                output.AddRange(lines);
                output.Add($"[{ShellSection}]");
                output.Add($"IconFile={icoPath}");
                output.Add("IconIndex=0");
            }
            else
            {
                var inserted = false;
                for (var index = 0; index < lines.Count; index++)
                {
                    var line = lines[index];
                    var trimmed = line.Trim();
                    var inShell = sectionNames[index]?.Equals(ShellSection, StringComparison.OrdinalIgnoreCase) == true;
                    var startsNextSection = trimmed.StartsWith('[') &&
                        !trimmed[1..^1].Trim().Equals(ShellSection, StringComparison.OrdinalIgnoreCase);
                    if (!inserted && startsNextSection && output.Any(item => item.Trim().Equals($"[{ShellSection}]", StringComparison.OrdinalIgnoreCase)))
                    {
                        AddIconLines(output, icoPath);
                        inserted = true;
                    }

                    if (inShell && TryGetKey(trimmed, out var key) &&
                        key is "iconfile" or "iconindex" or "iconresource")
                        continue;
                    output.Add(line);
                }
                if (!inserted) AddIconLines(output, icoPath);
            }

            var resultText = string.Join(newline, output) + newline;
            return DesktopIniMergeResult.Succeeded(Encode(resultText, decoded.Encoding!, decoded.HasBom));
        }
        catch (Exception exception) when (exception is DecoderFallbackException or EncoderFallbackException or ArgumentException)
        {
            return DesktopIniMergeResult.Failure(exception.Message);
        }
    }

    private static void AddIconLines(List<string> lines, string icoPath)
    {
        lines.Add($"IconFile={icoPath}");
        lines.Add("IconIndex=0");
    }

    private static bool TryGetKey(string line, out string key)
    {
        var separator = line.IndexOf('=');
        key = separator > 0 ? line[..separator].Trim().ToLowerInvariant() : string.Empty;
        return separator > 0;
    }

    private static (bool Success, string? Text, Encoding? Encoding, bool HasBom, string? Error) Decode(byte[] bytes)
    {
        if (bytes.Contains((byte)0) && !(bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE))
            return (false, null, null, false, "The document contains unsupported NUL bytes.");

        Encoding encoding;
        var offset = 0;
        var hasBom = false;
        if (bytes.AsSpan().StartsWith(Encoding.Unicode.GetPreamble()))
        {
            encoding = new UnicodeEncoding(false, false, true);
            offset = 2;
            hasBom = true;
        }
        else if (bytes.AsSpan().StartsWith(Encoding.UTF8.GetPreamble()))
        {
            encoding = new UTF8Encoding(false, true);
            offset = 3;
            hasBom = true;
        }
        else
        {
            encoding = new UTF8Encoding(false, true);
        }

        try { return (true, encoding.GetString(bytes, offset, bytes.Length - offset), encoding, hasBom, null); }
        catch (DecoderFallbackException exception) { return (false, null, null, false, exception.Message); }
    }

    private static byte[] Encode(string text, Encoding encoding, bool includeBom)
    {
        var content = encoding.GetBytes(text);
        if (!includeBom) return content;
        var preamble = encoding.CodePage == Encoding.Unicode.CodePage
            ? Encoding.Unicode.GetPreamble()
            : Encoding.UTF8.GetPreamble();
        return preamble.Concat(content).ToArray();
    }
}
