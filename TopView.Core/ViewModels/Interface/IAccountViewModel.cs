using System.Collections.ObjectModel;
using TopView.Core.Models;
using TopView.Core.ViewModels;

namespace TopView.Core.ViewModels.Interface
{
    public interface IAccountViewModel
    {
        public string Name { get; set; }
        public decimal Assets { get; set; }
        public decimal Balance { get; set; }
        public decimal Cash { get; set; }
        public decimal Realized { get; set; }
        public decimal Unrealized { get; set; }
        public Account? Account { get; set; }
        public ObservableCollection<TradeViewModel> Trades { get; }
        void Update();
    }
}
