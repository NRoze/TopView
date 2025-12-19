using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopView.Model.Models
{
    public class BalancePoint
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public double Balance { get; set; }
    }
}
