using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopView.Core.Infrastructure;
using TopView.Core.Models;
using TopView.Core.Services;
using TopView.Core.ViewModel;

namespace TopView.Core.ViewModels
{
    public class OverviewViewModel : BaseNotify, IAccountViewModel
    {
        private readonly AccountsViewModel _accountsViewModel;
        private readonly IAccountRepository _repo;
        public OverviewViewModel(IAccountRepository repo, AccountsViewModel vm)
        {
            _repo = repo;
            _accountsViewModel = vm;
        }

        private decimal _totalBalance;
        public decimal Balance { get => _totalBalance; set => SetProperty(ref _totalBalance, value); }

        private decimal _totalCash;
        public decimal Cash { get => _totalCash; set => SetProperty(ref _totalCash, value); }

        private decimal _totalAssets;
        public decimal Assets { get => _totalAssets; set => SetProperty(ref _totalAssets, value); }

        private decimal _realized;
        public decimal Realized { get => _realized; set => SetProperty(ref _realized, value); }

        private decimal _unrealized;
        public decimal Unrealized { get => _unrealized; set => SetProperty(ref _unrealized, value); }

        private double _successRate;
        public double SuccessRate { get => _successRate; set => SetProperty(ref _successRate, value); }

        private int _totalTrades;
        public int TotalTrades { get => _totalTrades; set => SetProperty(ref _totalTrades, value); }

        private double _averageReturn;
        public double AverageReturn { get => _averageReturn; set => SetProperty(ref _averageReturn, value); }
        public Account? Account { get; set; }

        public ObservableCollection<TradeViewModel> Trades { get; }
        public string Name { get; set; }

        public async void Update()
        {
            var accounts = _accountsViewModel.Accounts.Where(a => !a.Account.IsOverview).ToList();

            Balance = accounts.Sum(a => a.Balance);
            Cash = accounts.Sum(a => a.Cash);
            Assets = accounts.Sum(a => a.Assets);
            Realized = accounts.Sum(a => a.Realized);
            Unrealized = accounts.Sum(a => a.Unrealized);

            var allTrades = await _repo.GetTradesAsync();

            if (allTrades.Count() > 0)
            {
                var successful = allTrades.Count(t => t.Realized > 0);
                var total = allTrades.Count();

                SuccessRate = total > 0 ? (double)successful / total * 100 : 0;
                TotalTrades = total;
                AverageReturn = total > 0 ? (double)allTrades.Average(t => t.Realized) : 0;
            }
        }
    }
}