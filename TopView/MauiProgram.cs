using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TopView.Common.Infrastructure;
using TopView.Model.Models;
using TopView.Model.Data;
using TopView.Services.Interfaces;
using TopView.Services;
using TopView.ViewModel.Interfaces;
using TopView.ViewModel;
using TopView.Services.Services.Decorators;
using TopView.Extensions;

namespace TopView
{
    public static class MauiProgram
    {
        public static IServiceCollection? Services { get; private set; }

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddMemoryCache();
            builder.Services.AddRepositories();
            builder.Services.AddServices();
            builder.Services.AddDbContext();
            builder.Services.AddViewModels();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            ServiceHelper.Initialize(app.Services);
            ServiceHelper.GetService<IHeartbeatService>().Start();

            return app;
        }
    }
}
