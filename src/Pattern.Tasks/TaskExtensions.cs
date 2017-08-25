using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pattern.Tasks
{
    public static class TaskExtensions
    {
        public static IHandleError DefaultHandler { get; set; }

        public static Task FireAsync(this Task task, CancellationToken cancellationToken)
        {
            return Task.Run(() => task.FireAsyncInternal(), cancellationToken);
        }

        private static async Task FireAsyncInternal(this Task task)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                DefaultHandler.Handle(ex);
            }
        }

        public static async void Fire(this Task task, ILoadingHandler loadingHandler = null)
        {
            loadingHandler?.StartLoading();
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                DefaultHandler.Handle(ex);
            }
            finally
            {
                loadingHandler?.StopLoading();
            }
        }
    }
}
