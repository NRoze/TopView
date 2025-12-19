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
    public class OverviewViewModel(IDataRepository dataRepo, IAccountRepository repo, AccountsViewModel vm) : BaseNotify, IAccountViewModel
    {
        private readonly IDataRepository _dataRepo = dataRepo;
        private readonly AccountsViewModel _accountsViewModel = vm;
        private readonly IAccountRepository _accountRepo = repo;
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
        public decimal LastMonthBalance => (decimal)(BalancePoints?.LastOrDefault()?.Balance ?? 0);
        public Account? Account { get; set; }

        public ObservableCollection<TradeViewModel> Trades { get; }
        public string Name { get; set; }

        public async Task Update()
        {
            var accounts = _accountsViewModel.Accounts.Where(a => a.Account?.IsOverview != true).ToList();

            Balance = accounts.Sum(a => a.Balance);
            Cash = accounts.Sum(a => a.Cash);
            Assets = accounts.Sum(a => a.Assets);
            Realized = accounts.Sum(a => a.Realized);
            Unrealized = accounts.Sum(a => a.Unrealized);

            var allTrades = await _accountRepo.GetTradesAsync();

            if (allTrades.Count() > 0)
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
            await createIfNeeded();
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
            BalancePoints.Add(newPoint);
        }

        private async Task updateBalancePoint(BalancePoint point)
        {
            point.Balance = (double)Balance;
            await _dataRepo.SaveBalancePointAsync(point);
        }

        private async Task createIfNeeded()
        {
            var monthlyData = await _dataRepo.GetBalancePointsAsync();

            if (monthlyData != null && monthlyData.Count() > 0)
            {
                BalancePoints = [.. monthlyData!];
            }
            else
            {
                BalancePoints = new List<BalancePoint>();
            };
        }
    }
}