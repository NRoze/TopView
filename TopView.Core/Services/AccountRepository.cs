using TopView.Core.Data;
using TopView.Core.Models;

namespace TopView.Core.Services;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _db;

    public AccountRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task Reset() => await _db.Reset();
    public async Task<IEnumerable<Account>> GetAccountsAsync() => await _db.GetAccountsAsync();
    public async Task<IEnumerable<Trade>> GetTradesAsync() => await _db.GetTradesAsync();

    public async Task AddAsync(Account account) => await _db.SaveAccountAsync(account);

    public async Task RemoveAsync(Account account) => await _db.DeleteAccountAsync(account);
}
