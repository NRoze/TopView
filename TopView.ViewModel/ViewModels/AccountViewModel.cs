using System.Collections.ObjectModel;
using TopView.ViewModel.Interfaces;
using TopView.Model.Models;
using TopView.Services.Interfaces;
using TopView.Common.Infrastructure;
using TopView.Common.Interfaces;

namespace TopView.ViewModel
{
    public class AccountViewModel : BaseNotify, IAccountViewModel
    {
        private readonly IStockService _stockService;
        private readonly RepositoryCached<Trade> _repo;
        private readonly RepositoryCached<Account> _accountRepo;
        public ObservableCollection<TradeViewModel> Trades { get; } = new ObservableCollection<TradeViewModel>();
        public AddTradeViewModel AddTradeViewModel { get; } = new AddTradeViewModel();

        public Account? Account
        {
            get => field;
            set
            {
                if (field != value)
                {
                    field = value;
                    updateValues();
                    updateTrades();
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get { return field; }
            set
            {
                if (field != value)
                {
                    Account?.Name = field = value;
                    OnPropertyChanged();
                    Commit();
                }
            }
        }
        public void Commit()
        {
            if (Account != null)
            {
                _accountRepo.SaveAsync(Account);
            }
        }
        public AccountViewModel(
            RepositoryCached<Account> accountRepo,
            RepositoryCached<Trade> repo, 
            IStockService stockService,
            IHeartbeatService heartbeatService)
        {
            _accountRepo = accountRepo;
            _repo = repo;
            _stockService = stockService;
            heartbeatService.Ticked += async (date) => await Update();
            AddTradeViewModel.SubmitAction += AddTransaction;
        }

        public RelayCommand<decimal> SubmitTransferCommand => new RelayCommand<decimal>(tryTranferCash);
        private void tryTranferCash(decimal i_Amount)
        {
            Cash += i_Amount;
        }
        private async Task AddTransaction(string i_Symbol, decimal i_Quantity, decimal i_Price, bool i_IsBuy)
        {
            Trade? trade = Account?.Trades.FirstOrDefault(t => t.Symbol == i_Symbol && !t.IsOver);

            if (trade == null)
            {
                if (i_IsBuy)
                {
                    await addNewTrade(i_Symbol, i_Quantity, i_Price);
                }
            }
            else
            {
                await editExistingTrade(trade, i_Quantity, i_Price, i_IsBuy);
            }
        }

        private async Task editExistingTrade(Trade i_Trade, decimal i_Quantity, decimal i_Price, bool i_IsBuy)
        {
            if (i_Trade != null)
            {
                TradeViewModel? viewModel = Trades.FirstOrDefault(t => t.Symbol == i_Trade.Symbol);

                if (viewModel != null)
                {
                    if (i_IsBuy)
                    {
                        Cash -= i_Quantity * i_Price;
                        viewModel.Cost = ((i_Trade.Cost * i_Trade.Quantity) + (i_Price * i_Quantity)) / (i_Trade.Quantity + i_Quantity);
                    }
                    if (!i_IsBuy)
                    {
                        Cash += i_Quantity * i_Price;
                        viewModel.Realized += i_Quantity * (i_Price - i_Trade.Cost);
                        Realized += i_Quantity * (i_Price - i_Trade.Cost);
                    }

                    i_Trade.Quantity += i_IsBuy ? i_Quantity : -i_Quantity;
                    viewModel.UpdateFromModel();
                    if (i_Trade.Quantity <= 0)
                    {
                        tradeCompleted(viewModel);
                    }
                    else
                    {
                        await updateTrade(viewModel);
                    }
                
                    UpdateAssets();
                }
            }
        }

        private void tradeCompleted(TradeViewModel i_ViewModel)
        {
            i_ViewModel.Trade.IsOver = true;
            i_ViewModel.Commit();
            Trades.Remove(i_ViewModel);
        }

        private async Task addNewTrade(string i_Symbol, decimal i_Quantity, decimal i_Price)
        {
            var trade = new Trade
            {
                AccountId = Account.Id,
                Symbol = i_Symbol,
                Quantity = i_Quantity,
                Date = DateTime.Now,
                Cost = i_Price
            };
            await _repo.AddAsync(trade);

            var viewModel = new TradeViewModel(_repo, trade);

            Cash -= i_Quantity * i_Price;
            Trades.Add(viewModel);
            Account.Trades.Add(trade);
            updateTrade(viewModel);
        }

        private void updateValues()
        {
            Name = Account.Name;
            Commit();
        }

        private void updateTrades()
        {
            Trades.Clear();
            if (Account != null)
            {
                foreach (var trade in Account.Trades)
                {
                    var viewModel = new TradeViewModel(_repo, trade);

                    Trades.Add(viewModel);
                }
            }
        }

        public async Task Update()
        {
            await Parallel.ForEachAsync(Trades, new ParallelOptions { MaxDegreeOfParallelism = 5 }, 
                async (tradeVM, token) =>
            {
                try
                {
                    await updateTrade(tradeVM);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating {tradeVM.Trade.Symbol}: {ex.Message}");
                }
            });
        }

        private async Task updateTrade(TradeViewModel tradeVM)
        {
            var quote = await _stockService.GetQuoteAsync(tradeVM.Symbol);
            
            if (quote != null)
            {
                tradeVM.Price = quote.c;
                tradeVM.Change = quote.d; 
                tradeVM.ChangeP = quote.dp;
                UpdateAssets();
            }
        }

        private void UpdateAssets()
        {
            Assets = Trades.Select(t => t.Value).Sum();
            Commit();
        }

        public decimal Cash
        {
            get => Account != null ? Account.Cash : 0;
            set
            {
                if (Account != null && Account.Cash != value)
                {
                    Account.Cash = value;
                    OnPropertyChanged();
                    UpdateBalance();
                    Commit();
                }
            }
        }

        private void UpdateBalance()
        {
            Balance = Cash + Assets;
        }

        public decimal Assets
        {
            get => Account != null ? Account.Assets : 0;
            set
            {
                if (Account != null && Account.Assets != value)
                {
                    Account.Assets = value;
                    OnPropertyChanged();
                    UpdateBalance();
                    OnPropertyChanged(nameof(Unrealized));
                    Commit();
                }
            }
        }
        public decimal Balance
        {
            get => Account != null ? Account.Balance : 0;
            set
            {
                if (Account != null && Account.Balance != value)
                {
                    Account.Balance = value;
                    OnPropertyChanged();
                    Commit();
                }
            }
        }
        public decimal Unrealized
        {
            set { }
            get
            {
                return Trades.Select(t => t.Unrealized).Sum();
            }
        }

        public decimal Realized
        {
            get => Account != null ? Account.Realized : 0;
            set
            {
                if (Account != null && Account.Realized != value)
                {
                    Account.Realized = value;
                    OnPropertyChanged();
                    Commit();
                }
            }
        }
    }
}
