using SQLite;
using TopView.Model.Models;

namespace TopView.Model.Data;

public class AppDbContext
{
    private readonly SQLiteAsyncConnection _db;

    public AppDbContext(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
        _db.CreateTableAsync<Account>().Wait();
        _db.CreateTableAsync<Trade>().Wait();
        _db.CreateTableAsync<BalancePoint>().Wait();
    }

    public async Task Reset() 
    { 
        await _db.DeleteAllAsync<Account>();
        await _db.DeleteAllAsync<Trade>();
        await _db.DeleteAllAsync<BalancePoint>();
    }
    public async Task<List<Account>> GetAccountsAsync() => await _db.Table<Account>().ToListAsync();
    public async Task<List<Trade>> GetTradesAsync() => await _db.Table<Trade>().ToListAsync();
    public async Task<List<BalancePoint>> GetBalancePointsAsync() => await _db.Table<BalancePoint>().ToListAsync();
    public async Task<int> SaveAsync<T>(T item) => await _db.InsertOrReplaceAsync(item);
    public async Task<int> DeleteAsync<T>(T item) => await _db.DeleteAsync(item);
    public async Task<int> AddBalancePointAsync(BalancePoint account) => await _db.InsertAsync(account);
    public async Task<int> SaveBalancePointAsync(BalancePoint account) => await _db.InsertOrReplaceAsync(account);
    public async Task<int> DeleteBalancePointAsync(BalancePoint account) => await _db.DeleteAsync(account);
    public async Task<int> AddAccountAsync(Account account) => await _db.InsertAsync(account);
    public async Task<int> SaveAccountAsync(Account account) => await _db.InsertOrReplaceAsync(account);
    public async Task<int> DeleteAccountAsync(Account account) => await _db.DeleteAsync(account);
    public async Task<int> AddTradeAsync(Trade trade) => await _db.InsertAsync(trade);
    public async Task<int> SaveTradeAsync(Trade trade) => await _db.InsertOrReplaceAsync(trade);
    public async Task<int> DeleteTradeAsync(Trade trade) => await _db.DeleteAsync(trade);
}
