using System.Collections.ObjectModel;
using TopView.Model.Models;

namespace TopView.ViewModels.Interfaces
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
        Task Update();
    }
}
