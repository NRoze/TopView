using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopView.Core.Models;

namespace TopView.Core.Services
{
    public interface IDataRepository
    {
        Task<IEnumerable<BalancePoint>> GetBalancePointsAsync();
        Task AddBalancePointAsync(BalancePoint account);
        Task SaveBalancePointAsync(BalancePoint account);
        Task RemoveBalancePointAsync(BalancePoint account);
    }
}
