
namespace TopView
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent(); 
            
            AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
        void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogAndHandleException(e.ExceptionObject as Exception, "AppDomain");
        }

        private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            LogAndHandleException(e.Exception, "TaskScheduler");
            e.SetObserved(); // Prevent process termination
        }

        private void LogAndHandleException(Exception? ex, string source)
        {
            if (ex == null) return;

            // Log locally
            System.Diagnostics.Debug.WriteLine($"🔥 Unhandled exception ({source}): {ex}");

            // TODO: Add production handling here (AppCenter, Sentry, etc.)
            // Example: _logger.LogError(ex, "Unhandled exception ({Source})", source);

            // Optional: show an alert to user
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await Shell.Current.DisplayAlert("Unexpected Error",
                        "An unexpected error occurred. The app will continue running.", "OK");
                }
                catch { /* might fail early in app lifecycle */ }
            });
        }
    }
}