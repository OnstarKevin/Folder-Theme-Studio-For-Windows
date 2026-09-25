using System.Windows;
using Point = System.Windows.Point;

namespace FolderThemeStudio.App.Services;

public interface IExplorerFolderHoverTargetResolver
{
    ExplorerHoverTarget? TryResolve(Point screenPoint);
}

public sealed class FolderHoverDecision
{
    private readonly TimeSpan dwell;
    private string? candidatePath;
    private DateTimeOffset candidateSince;

    public FolderHoverDecision(TimeSpan? dwell = null)
    {
        this.dwell = dwell ?? TimeSpan.FromMilliseconds(550);
        if (this.dwell < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(dwell));
    }

    public FolderCategoryEntry? Update(
        ExplorerHoverTarget? target,
        Point screenPoint,
        IReadOnlyDictionary<string, FolderCategoryEntry> entries,
        DateTimeOffset now)
    {
        if (target is null || !target.ItemBounds.Contains(screenPoint) ||
            !entries.TryGetValue(target.FolderPath, out var entry))
        {
            Reset();
            return null;
        }

        if (!string.Equals(candidatePath, target.FolderPath, StringComparison.OrdinalIgnoreCase))
        {
            candidatePath = target.FolderPath;
            candidateSince = now;
        }

        return now - candidateSince >= dwell ? entry : null;
    }

    public void Reset()
    {
        candidatePath = null;
        candidateSince = default;
    }
}
