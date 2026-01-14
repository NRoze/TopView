
using TopView.Model.Data;
using TopView.Model.Models;
using TopView.Services.Interfaces;

namespace TopView.Services;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _db;

    public AccountRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task Reset() => await _db.Reset();
    public async Task<List<Account>?> GetAccountsAsync() => await _db.GetAccountsAsync();

    public async Task AddAsync(Account account) => await _db.AddAccountAsync(account);
    public async Task SaveAsync(Account account) => await _db.SaveAccountAsync(account);

    public async Task RemoveAsync(Account account) => await _db.DeleteAccountAsync(account);
}
