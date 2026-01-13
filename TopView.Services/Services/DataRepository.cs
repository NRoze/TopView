using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopView.Model.Data;
using TopView.Model.Models;
using TopView.Services.Interfaces;

namespace TopView.Services
{
    public class DataRepository : IDataRepository
    {
        private readonly AppDbContext _db;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "balance_points";
        public DataRepository(AppDbContext db, IMemoryCache cache)
        {
            _db = db;
            _cache = cache;
        }

        public async Task<List<BalancePoint>> GetBalancePointsAsync()
        {
            if (!_cache.TryGetValue(CacheKey, out List<BalancePoint> balances))
            {
                balances = await _db.GetBalancePointsAsync().ConfigureAwait(false);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                _cache.Set(CacheKey, balances, cacheOptions);
            }

            return balances;
        }
        public async Task AddBalancePointAsync(BalancePoint bp) => await _db.AddBalancePointAsync(bp);
        public async Task RemoveBalancePointAsync(BalancePoint bp) => await _db.DeleteBalancePointAsync(bp);
        public async Task SaveBalancePointAsync(BalancePoint bp) => await _db.SaveBalancePointAsync(bp);
    }
}
