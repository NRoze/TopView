using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopView.Model.Models
{
    public class Trade
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; init; }
        public int AccountId { get; init; }        

        [MaxLength(10)]
        public string Symbol { get; init; } = string.Empty;

        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public decimal Realized { get; set; }
        public decimal Quantity { get; set; }
        public decimal Change { get; set; }
        public decimal ChangeP { get; set; }
        public DateTime Date { get; set; }
        public bool IsOver { get; set; }

    }
}
