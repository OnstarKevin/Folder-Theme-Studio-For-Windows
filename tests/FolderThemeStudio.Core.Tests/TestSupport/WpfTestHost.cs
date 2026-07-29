using System.Collections.Concurrent;
using System.Runtime.ExceptionServices;

namespace FolderThemeStudio.Core.Tests.TestSupport;

internal static class WpfTestHost
{
    private static readonly Lazy<Host> Instance = new(() => new Host());

    internal static void Invoke(Action action) => Instance.Value.Invoke(action);

    private sealed class Host
    {
        private readonly BlockingCollection<WorkItem> queue = [];

        internal Host()
        {
            var ready = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var thread = new Thread(() =>
            {
                var application = new FolderThemeStudio.App.App();
                application.InitializeComponent();
                ready.SetResult();
                foreach (var item in queue.GetConsumingEnumerable()) item.Execute();
            })
            {
                IsBackground = true,
                Name = "Folder Theme Studio WPF test host",
            };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            ready.Task.WaitAsync(TimeSpan.FromSeconds(10)).GetAwaiter().GetResult();
        }

        internal void Invoke(Action action)
        {
            var item = new WorkItem(action);
            queue.Add(item);
            item.Wait();
        }
    }

    private sealed class WorkItem(Action action)
    {
        private readonly TaskCompletionSource completion = new(TaskCreationOptions.RunContinuationsAsynchronously);
        private Exception? failure;

        internal void Execute()
        {
            try { action(); }
            catch (Exception exception) { failure = exception; }
            finally { completion.SetResult(); }
        }

        internal void Wait()
        {
            completion.Task.WaitAsync(TimeSpan.FromSeconds(10)).GetAwaiter().GetResult();
            if (failure is not null) ExceptionDispatchInfo.Capture(failure).Throw();
        }
    }
}
