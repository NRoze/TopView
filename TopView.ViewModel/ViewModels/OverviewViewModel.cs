using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopView.Common.Infrastructure;
using TopView.Model.Models;
using TopView.Services.Interfaces;
using TopView.ViewModel.Interfaces;

namespace TopView.ViewModel
{
    public class OverviewViewModel : BaseNotify, IAccountViewModel
    {
        private readonly IDataRepository _dataRepo;
        private readonly AccountsViewModel _accountsViewModel;
        private readonly IAccountRepository _accountRepo;
        private readonly ITradeRepository _tradeRepo;
        public List<BalancePoint>? BalancePoints { get; private set; }

        public decimal Balance
        {
            get => field;
            set
            {
                SetProperty(ref field, value);
                var handler = udateBalancePoint;
                if (handler != null)
                {
                    _ = handler();
                }
            }
        }

        public decimal Cash { get => field; set => SetProperty(ref field, value); }

        public decimal Assets { get => field; set => SetProperty(ref field, value); }

        public decimal Realized { get => field; set => SetProperty(ref field, value); }

        public decimal Unrealized { get => field; set => SetProperty(ref field, value); }

        public double SuccessRate { get => field; set => SetProperty(ref field, value); }

        public int TotalTrades { get => field; set => SetProperty(ref field, value); }

        private double _averageReturn;

        public OverviewViewModel(
            IDataRepository dataRepo, 
            IAccountRepository repo, 
            ITradeRepository tradeRepo, 
            AccountsViewModel vm)
        {
            _dataRepo = dataRepo;
            _accountsViewModel = vm;
            _accountRepo = repo;
            _tradeRepo = tradeRepo;
        }

        public double AverageReturn { get => _averageReturn; set => SetProperty(ref _averageReturn, value); }
        public decimal DaylyReturn
        {
            get
            {
                decimal result = 0;

                if (_accountsViewModel?.Accounts?.Count > 0)
                {
                    result = _accountsViewModel.Accounts.Where(a => a.Trades?.Count > 0)
                        .Sum(a => a.Trades.Sum(t => t.Change));
                }

                return result;
            }
        }

        public decimal DaylyReturnP => (Balance - DaylyReturn > 0) ? DaylyReturn / (Balance - DaylyReturn) : 0;
        public decimal MonthlyReturn => Balance - LastMonthBalance;
        public decimal MonthlyReturnP => LastMonthBalance > 0 ? MonthlyReturn / LastMonthBalance : 1;
        public decimal LastMonthBalance => (decimal)(BalancePoints?.Where(x => x.Time.Month != DateTime.UtcNow.Month).LastOrDefault()?.Balance ?? 0);
        public Account? Account { get; set; }

        public ObservableCollection<TradeViewModel> Trades { get; } // inheritence requirement
        public string Name { get; set; }

        public async Task Update()
        {
            var accounts = _accountsViewModel.Accounts.Where(a => a.Account?.IsOverview != true).ToList();

            Balance = accounts.Sum(a => a.Balance);
            Cash = accounts.Sum(a => a.Cash);
            Assets = accounts.Sum(a => a.Assets);
            Realized = accounts.Sum(a => a.Realized);
            Unrealized = accounts.Sum(a => a.Unrealized);

            var allTrades = await _tradeRepo.GetTradesAsync();

            if (allTrades?.Count() > 0)
            {
                var successful = allTrades.Count(t => t.Realized > 0);
                var total = allTrades.Count();

                SuccessRate = total > 0 ? (double)successful / total * 100 : 0;
                TotalTrades = total;
                AverageReturn = total > 0 ? (double)allTrades.Average(t => t.Realized) : 0;
            }
        }

        private async Task udateBalancePoint()
        {
            await Initialize();
            await updateCurrentBalancePoint();
            OnPropertyChanged(nameof(BalancePoints));
        }

        private async Task updateCurrentBalancePoint()
        {
            var today = DateTime.Today;
            var point = BalancePoints?.FirstOrDefault(
                bp => bp.Time.Month == today.Month &&
                        bp.Time.Year == today.Year);

            if (point != null)
            {
                await updateBalancePoint(point);
            }
            else
            {
                await addBalancePoint(today);
            }
        }

        private async Task addBalancePoint(DateTime today)
        {
            BalancePoint newPoint = new BalancePoint { Time = today, Balance = (double)Balance };

            await _dataRepo.AddBalancePointAsync(newPoint);
            BalancePoints?.Add(newPoint);
        }

        private async Task updateBalancePoint(BalancePoint point)
        {
            point.Balance = (double)Balance;
            await _dataRepo.SaveBalancePointAsync(point);
        }

        private async Task Initialize()
        {
            var monthlyData = await _dataRepo.GetBalancePointsAsync();

            if (monthlyData != null && monthlyData.Count() > 0)
            {
                BalancePoints = [.. monthlyData!];
                OnPropertyChanged(nameof(MonthlyReturnP));
            }
            else
            {
                BalancePoints = new List<BalancePoint>();
            }
        }
    }
}