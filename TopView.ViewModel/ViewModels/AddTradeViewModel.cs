using System.Windows.Input;
using TopView.Common.Infrastructure;

namespace TopView.ViewModel
{
    public class AddTradeViewModel : BaseNotify
    {
        public RelayCommand SubmitCommand { get; }
        public Func<string, decimal, decimal, bool, Task> SubmitAction;
        public AddTradeViewModel()
        {
            SubmitCommand = new RelayCommand(OnSubmit, CanSubmit);
        }

        private void OnSubmit(object? obj)
        {
           SubmitAction?.Invoke(Symbol, _quantityParsed, _priceParsed, IsBuy);
        }

        private bool CanSubmit(object? obj)
        {
            return !string.IsNullOrWhiteSpace(Symbol) && 
                decimal.TryParse(Quantity, out _quantityParsed) && 
                decimal.TryParse(Price, out _priceParsed);
        }

        private string _symbol;
        public string Symbol
        {
            get => _symbol;
            set
            {
                SetProperty(ref _symbol, value);
                SubmitCommand.NotifyCanExecuteChanged(); 
            }
        }
        private decimal _priceParsed;
        private string _price;
        public string Price
        {
            get => _price;
            set
            {
                SetProperty(ref _price, value);
                SubmitCommand.NotifyCanExecuteChanged();
            }
        }
        private decimal _quantityParsed;
        private string _quantity;
        public string Quantity
        {
            get => _quantity;
            set
            {
                SetProperty(ref _quantity, value);
                SubmitCommand.NotifyCanExecuteChanged();
            }
        }

        private bool _isBuy = true;

        public bool IsBuy
        {
            get { return _isBuy; }
            set
            {
                if (_isBuy != value)
                {
                    _isBuy = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand TradeToggleCommand => new RelayCommand(_ => IsBuy = !IsBuy);
    }
}
