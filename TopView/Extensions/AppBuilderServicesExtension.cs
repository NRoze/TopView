using TopView.Common.Infrastructure;
using TopView.Model.Data;
using TopView.Model.Models;
using TopView.Services;
using TopView.Services.Interfaces;
using TopView.Services.Services.Decorators;
using TopView.ViewModel;
using TopView.ViewModel.Interfaces;

namespace TopView.Extensions
{
    public static class AppBuilderServicesExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IDataRepository, DataRepositoryCached>();
            services.AddSingleton<DataRepository>();
            services.AddSingleton<IAccountRepository, AccountRepositoryCached>();
            services.AddSingleton<AccountRepository>();
            services.AddSingleton<ITradeRepository, TradeRepositoryCached>();
            services.AddSingleton<TradeRepository>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IHeartbeatService, HeartbeatService>();
            services.AddSingleton<IDialogService, MauiDialogService>();
            services.AddSingleton<ISettingsPageViewModel, SettingsPageViewModel>();
            services.AddSingleton<IStockService>(sp =>
            {
                string apiKey = "c6cf25iad3i95gi9b870";
                return new StockService(apiKey);
            });
        }
        public static void AddDbContext(this IServiceCollection services)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "topview.db3");

            services.AddSingleton(new AppDbContext(dbPath));
        }
        public static void AddViewModels(this IServiceCollection services)
        {
            services.AddSingleton<IViewModelFactory, ViewModelFactory>();
            services.AddSingleton<AccountsViewModel>();
            services.AddTransient<OverviewViewModel>();
            services.AddTransient<Func<Account, AccountViewModel>>(sp =>
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
        }
    }
}
