using SQLite;
using TopView.Model.Models;

namespace TopView.Model.Data;

public class AppDbContext
{
    private readonly SQLiteAsyncConnection _db;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly Task _initializationTask;

    public AppDbContext(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
        _initializationTask = InitializeTables();
    }
    public async Task InitializeTables()
    {
        await ExecuteAction(() => _db.CreateTablesAsync<Account, Trade, BalancePoint>());
    }
    public async Task<T> ExecuteAction<T>(Func<Task<T>> action)
    {
        await _initializationTask;
        await _semaphore.WaitAsync();

        try
        {
            return await action();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task Reset() 
    { 
        await ExecuteAction(() => _db.DeleteAllAsync<Account>());
        await ExecuteAction(() => _db.DeleteAllAsync<Trade>());
        await ExecuteAction(() => _db.DeleteAllAsync<BalancePoint>());
    }
    public async Task<List<Account>> GetAccountsAsync() => await ExecuteAction(() => _db.Table<Account>().ToListAsync());
    public async Task<List<Trade>> GetTradesAsync() => await ExecuteAction(() =>  _db.Table<Trade>().ToListAsync());
    public async Task<List<BalancePoint>> GetBalancePointsAsync() => await ExecuteAction(() => _db.Table<BalancePoint>().ToListAsync());
    public async Task<int> SaveAsync<T>(T item) => await ExecuteAction<int>(() => _db.InsertOrReplaceAsync(item));
    public async Task<int> DeleteAsync<T>(T item) => await ExecuteAction<int>(() => _db.DeleteAsync(item));
    public async Task<int> AddBalancePointAsync(BalancePoint account) => await ExecuteAction(() => _db.InsertAsync(account));
    public async Task<int> SaveBalancePointAsync(BalancePoint account) => await ExecuteAction<int>(() => _db.InsertOrReplaceAsync(account));
    public async Task<int> DeleteBalancePointAsync(BalancePoint account) => await ExecuteAction<int>(() => _db.DeleteAsync(account));
    public async Task<int> AddAccountAsync(Account account) => await ExecuteAction<int>(() => _db.InsertAsync(account));
    public async Task<int> SaveAccountAsync(Account account) => await ExecuteAction<int>(() => _db.InsertOrReplaceAsync(account));
    public async Task<int> DeleteAccountAsync(Account account) => await ExecuteAction<int>(() => _db.DeleteAsync(account));
    public async Task<int> AddTradeAsync(Trade trade) => await ExecuteAction<int>(() => _db.InsertAsync(trade)); 
    public async Task<int> SaveTradeAsync(Trade trade) => await ExecuteAction<int>(() => _db.InsertOrReplaceAsync(trade));
    public async Task<int> DeleteTradeAsync(Trade trade) => await ExecuteAction<int>(() => _db.DeleteAsync(trade));
}
