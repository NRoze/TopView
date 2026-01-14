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

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "topview.db3");

            // Dependency injection
            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton(new AppDbContext(dbPath));
            builder.Services.AddSingleton<IDataRepository, DataRepositoryCached>();
            builder.Services.AddSingleton<DataRepository>();
            builder.Services.AddSingleton<IAccountRepository, AccountRepositoryCached>();
            builder.Services.AddSingleton<AccountRepository>();
            builder.Services.AddSingleton<IHeartbeatService, HeartbeatService>();
            builder.Services.AddSingleton<IDialogService, MauiDialogService>();
            builder.Services.AddSingleton<ISettingsPageViewModel, SettingsPageViewModel>();
            builder.Services.AddSingleton<ITradeRepository, TradeRepository>();
            builder.Services.AddSingleton<IStockService>(sp =>
            {
                string apiKey = "c6cf25iad3i95gi9b870";
                return new StockService(apiKey);
            });

            builder.Services.AddSingleton<IViewModelFactory, ViewModelFactory>();
            builder.Services.AddSingleton<AccountsViewModel>();
            builder.Services.AddTransient<OverviewViewModel>();

            builder.Services.AddTransient<Func<Account, AccountViewModel>>(sp =>
            {
                return (Account account) =>
                {
                    var accountRepo = sp.GetRequiredService<IAccountRepository>();
                    var repo = sp.GetRequiredService<ITradeRepository>();
                    var stockService = sp.GetRequiredService<IStockService>();
                    var heartcheatService = sp.GetRequiredService<IHeartbeatService>();
                    return new AccountViewModel(accountRepo, repo, stockService, heartcheatService) { Account = account };
                };
            });

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
