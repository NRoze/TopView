using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TopView.Core.Data;
using TopView.Core.Models;
using TopView.Core.Services;
using TopView.Core.ViewModel;
using TopView.Core.ViewModels;
using TopView.Views;

namespace TopView
{
    public static class MauiProgram
    {
        public static IServiceCollection Services { get; private set; }

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
            builder.Services.AddSingleton(new AppDbContext(dbPath));
            builder.Services.AddSingleton<IAccountRepository, AccountRepository>();
            builder.Services.AddSingleton<ITradeRepository, TradeRepository>();
            builder.Services.AddSingleton<IStockService>(sp =>
            {
                string apiKey = "c6cf25iad3i95gi9b870";
                return new StockService(apiKey);
            });

            builder.Services.AddSingleton<AccountsViewModel>();
            //builder.Services.AddTransient<IAccountViewModel, AccountViewModel>();
            builder.Services.AddTransient<OverviewViewModel, OverviewViewModel>();

            builder.Services.AddTransient<Func<Account, AccountViewModel>>(sp =>
            {
                return (Account account) =>
                {
                    var accountRepo = sp.GetRequiredService<IAccountRepository>();
                    var repo = sp.GetRequiredService<ITradeRepository>();
                    var stockService = sp.GetRequiredService<IStockService>();
                    return new AccountViewModel(accountRepo, repo, stockService) { Account = account };
                };
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            ServiceHelper.Initialize(app.Services);

            return app;
        }
    }
}
