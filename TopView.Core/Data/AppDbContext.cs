using SQLite;
using TopView.Core.Models;

namespace TopView.Core.Data;

public class AppDbContext
{
    private readonly SQLiteAsyncConnection _db;

    public AppDbContext(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
        _db.CreateTableAsync<Account>().Wait();
        _db.CreateTableAsync<Trade>().Wait();
    }

    public async Task Reset() 
    { 
        await _db.DeleteAllAsync<Account>();
        await _db.DeleteAllAsync<Trade>();
    }
    public Task<List<Account>> GetAccountsAsync() => _db.Table<Account>().ToListAsync();
    public Task<List<Trade>> GetTradesAsync() => _db.Table<Trade>().ToListAsync();
    public Task<int> SaveAsync<T>(T item) => _db.InsertOrReplaceAsync(item);
    public Task<int> DeleteAsync<T>(T item) => _db.DeleteAsync(item);
    public Task<int> AddAccountAsync(Account account) => _db.InsertAsync(account);
    public Task<int> SaveAccountAsync(Account account) => _db.InsertOrReplaceAsync(account);
    public Task<int> DeleteAccountAsync(Account account) => _db.DeleteAsync(account);
    public Task<int> AddTradeAsync(Trade trade) => _db.InsertAsync(trade);
    public Task<int> SaveTradeAsync(Trade trade) => _db.InsertOrReplaceAsync(trade);
    public Task<int> DeleteTradeAsync(Trade trade) => _db.DeleteAsync(trade);
}
