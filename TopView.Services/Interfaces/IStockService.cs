using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopView.Model.Models;

namespace TopView.Services.Interfaces
{
    public interface IStockService
    {
        Task<Quote?> GetQuoteAsync(string symbol);
    }
}
