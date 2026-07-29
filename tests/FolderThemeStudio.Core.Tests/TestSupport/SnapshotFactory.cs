using FolderThemeStudio.Core.Recovery;
using Microsoft.Win32;

namespace FolderThemeStudio.Core.Tests.TestSupport;

internal static class SnapshotFactory
{
    private static readonly Guid FirstId = new("11111111-1111-1111-1111-111111111111");
    private static readonly Guid SecondId = new("22222222-2222-2222-2222-222222222222");
    private static readonly DateTimeOffset FirstCreatedAt = new(2026, 7, 27, 7, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset SecondCreatedAt = new(2026, 7, 27, 8, 0, 0, TimeSpan.Zero);

    internal static OperationSnapshot First() => new(
        FirstId,
        FirstCreatedAt,
        new RegistryValueSnapshot("3", true, "shell32.dll,3", RegistryValueKind.String),
        new RegistryValueSnapshot("4", false, null),
        [],
        false);

    internal static OperationSnapshot Second() => new(
        SecondId,
        SecondCreatedAt,
        new RegistryValueSnapshot("3", true, "imageres.dll,109", RegistryValueKind.String),
        new RegistryValueSnapshot("4", true, "imageres.dll,110", RegistryValueKind.String),
        [],
        false);

    internal static OperationSnapshot MixedRegistryState() => new(
        FirstId,
        FirstCreatedAt,
        new RegistryValueSnapshot("3", false, null),
        new RegistryValueSnapshot("4", true, "original.dll,4", RegistryValueKind.String),
        [],
        false);

    internal static OperationSnapshot WithRegistryAndFolderMutation() => First() with
    {
        Mutations = [FolderMutation()]
    };

    internal static FolderMutationRecord FolderMutation() => new(
        @"C:\Pictures\Holiday",
        System.IO.FileAttributes.Directory | System.IO.FileAttributes.Archive,
        @"C:\Pictures\Holiday\desktop.ini",
        "8C0D9E3F8C0D9E3F8C0D9E3F8C0D9E3F8C0D9E3F8C0D9E3F8C0D9E3F8C0D9E3F",
        System.IO.FileAttributes.ReadOnly,
        true);
}
