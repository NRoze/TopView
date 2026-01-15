using System.Diagnostics;

namespace TopView.Common.Extensions
{
    public static class AsyncSafe
    {
        public static void FireAndForgetSafeAsync(this Task task)
        {
            _ = task.ContinueWith(t =>
            {
                Debug.WriteLine($"An error occurred: {t.Exception?.Message}");
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
