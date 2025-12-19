using SQLite;
using System.Collections.ObjectModel;

namespace TopView.Model.Models
{
    public class Account
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public decimal Cash { get; set; }
        public decimal Assets { get; set; }
        public decimal Balance { get; set; }
        public decimal Realized { get; set; }
        public bool IsOverview { get; set; }

        [Ignore] 
        public ObservableCollection<Trade> Trades { get; set; } = new();
    }
}
