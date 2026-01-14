using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopView.Model.Models;

namespace TopView.Services.Interfaces
{
    public interface IDataRepository
    {
        Task<List<BalancePoint>?> GetBalancePointsAsync();
        Task AddBalancePointAsync(BalancePoint account);
        Task SaveBalancePointAsync(BalancePoint account);
        Task RemoveBalancePointAsync(BalancePoint account);
    }
}
