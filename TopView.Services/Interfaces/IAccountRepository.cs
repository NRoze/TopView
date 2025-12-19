using TopView.Model.Models;

namespace TopView.Services.Interfaces;

public interface IAccountRepository
{
    Task Reset();
    Task<IEnumerable<Account>> GetAccountsAsync();
    Task<IEnumerable<Trade>> GetTradesAsync();
    Task AddAsync(Account account);
    Task SaveAsync(Account account);
    Task RemoveAsync(Account account);
}
