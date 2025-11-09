using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopView.Core.Models;

namespace TopView.Core.Services
{
    public interface IStockService
    {
        Task<Quote?> GetQuoteAsync(string symbol);
    }
}
