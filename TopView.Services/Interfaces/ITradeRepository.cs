using TopView.Model.Models;

namespace TopView.Services.Interfaces
{
    public interface ITradeRepository
    {
        Task<List<Trade>?> GetTradesAsync();
        Task AddAsync(Trade account);
        Task SaveAsync(Trade account);
        Task RemoveAsync(Trade account);
    }
}
