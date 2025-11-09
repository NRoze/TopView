using System.Windows.Input;
using TopView.Core.Infrastructure;

namespace TopView.Core.ViewModels
{
    public class AddTradeViewModel : BaseNotify
    {
        public Command SubmitCommand { get; }
        public Func<string, decimal, decimal, bool, Task> SubmitAction;
        public AddTradeViewModel()
        {
            SubmitCommand = new Command(OnSubmit, CanSubmit);
        }

        private void OnSubmit()
        {
           SubmitAction.Invoke(Symbol, _quantityParsed, _priceParsed, IsBuy);
        }

        private bool CanSubmit()
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
                SubmitCommand.ChangeCanExecute(); 
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
                SubmitCommand.ChangeCanExecute();
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
                SubmitCommand.ChangeCanExecute();
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
        public ICommand TradeToggleCommand => new Command(_ => IsBuy = !IsBuy);
    }
}
