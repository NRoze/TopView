using Microsoft.Extensions.Caching.Memory;
using TopView.Model.Models;
using TopView.Services.Interfaces;

namespace TopView.Services.Services.Decorators
{
    public class DataRepositoryCached : IDataRepository
    {
        private readonly IMemoryCache _cache;
        private readonly IDataRepository _decorated;
        private const string CacheKey = "balance_points";
        public DataRepositoryCached(DataRepository decorated, IMemoryCache cache)
        {
            _decorated = decorated;
            _cache = cache;
        }
        public async Task<List<BalancePoint>?> GetBalancePointsAsync()
        {
            return await _cache.GetOrCreateAsync(
                CacheKey,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                    return _decorated.GetBalancePointsAsync();
                });
        }
        public async Task AddBalancePointAsync(BalancePoint bp)
        {
            await _decorated.AddBalancePointAsync(bp);
        }
        public async Task RemoveBalancePointAsync(BalancePoint bp)
        {
            await _decorated.RemoveBalancePointAsync(bp);
        }
        public async Task SaveBalancePointAsync(BalancePoint bp)
        {
            await _decorated.SaveBalancePointAsync(bp);
        }
    }
}
