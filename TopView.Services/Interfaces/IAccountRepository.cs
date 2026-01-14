using TopView.Model.Models;

namespace TopView.Services.Interfaces;

public interface IAccountRepository
{
    Task Reset();
    Task<List<Account>?> GetAccountsAsync();
    Task AddAsync(Account account);
    Task SaveAsync(Account account);
    Task RemoveAsync(Account account);
}
