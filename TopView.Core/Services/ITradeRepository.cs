using TopView.Core.Models;

namespace TopView.Core.Services
{
    public interface ITradeRepository
    {
        Task AddAsync(Trade account);
        Task SaveAsync(Trade account);
        Task RemoveAsync(Trade account);
    }
}
