using Microsoft.Extensions.Caching.Memory;
using TopView.Model.Models;
using TopView.Services.Interfaces;

namespace TopView.Services.Services.Decorators
{
    public class AccountRepositoryCached : IAccountRepository
    {
        private readonly IMemoryCache _cache;
        private readonly IAccountRepository _decorated;
        private const string CacheKey = "accounts";
        public AccountRepositoryCached(AccountRepository decorated, IMemoryCache cache)
        {
            _decorated = decorated;
            _cache = cache;
        }

        public async Task<List<Account>?> GetAccountsAsync()
        {
            return await _cache.GetOrCreateAsync(
                CacheKey,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                    return _decorated.GetAccountsAsync();
                });
        }


        public async Task AddAsync(Account account)
        {
            await _decorated.AddAsync(account);
        }

        public async Task SaveAsync(Account account)
        {
            await _decorated.SaveAsync(account);
        }

        public async Task RemoveAsync(Account account)
        {
            await _decorated.RemoveAsync(account);
        }
        public async Task Reset()
        {
            await _decorated.Reset();
        }
    }
}
