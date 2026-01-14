using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using TopView.Model.Models;
using TopView.Services.Interfaces;

namespace TopView.Services.Services.Decorators
{
    public class TradeRepositoryCached : ITradeRepository
    {
        private readonly IMemoryCache _cache;
        private readonly ITradeRepository _decorated;
        private const string CacheKey = "trades";
        public TradeRepositoryCached(TradeRepository decorated, IMemoryCache cache)
        {
            _decorated = decorated;
            _cache = cache;
        }

        public async Task<List<Trade>?> GetTradesAsync()
        {
            return await _cache.GetOrCreateAsync(
                CacheKey,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                    return _decorated.GetTradesAsync();
                });
        }

        public async Task AddAsync(Trade trade)
        {
            await _decorated.AddAsync(trade);
        }

        public async Task SaveAsync(Trade trade)
        {
            await _decorated.SaveAsync(trade);
        }

        public async Task RemoveAsync(Trade trade)
        {
            await _decorated.RemoveAsync(trade);
        }
    }
}
