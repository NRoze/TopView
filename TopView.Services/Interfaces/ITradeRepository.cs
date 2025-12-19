using TopView.Model.Models;

namespace TopView.Services.Interfaces
{
    public interface ITradeRepository
    {
        Task AddAsync(Trade account);
        Task SaveAsync(Trade account);
        Task RemoveAsync(Trade account);
    }
}
