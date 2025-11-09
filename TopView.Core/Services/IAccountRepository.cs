using TopView.Core.Models;

namespace TopView.Core.Services;

public interface IAccountRepository
{
    Task Reset();
    Task<IEnumerable<Account>> GetAccountsAsync();
    Task<IEnumerable<Trade>> GetTradesAsync();
    Task AddAsync(Account account);
    Task SaveAsync(Account account);
    Task RemoveAsync(Account account);
}
