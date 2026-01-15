using Microsoft.Extensions.Caching.Memory;
using TopView.Common.Interfaces;

namespace TopView.Common.Infrastructure
{
    public sealed class RepositoryCached<T> : IRepository<T> where T : class
    {
        private readonly IMemoryCache _cache;
        private readonly IRepository<T> _repository;
        private readonly string _cacheKey = typeof(T).Name;
        public RepositoryCached(IMemoryCache cache, IRepository<T> repository)
        {
            _cache = cache;
            _repository = repository;
        }
        public async Task<List<T>?> GetAllAsync()
        {
            return await _cache.GetOrCreateAsync(
                _cacheKey,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                    return _repository.GetAllAsync();
                });
        }
        public async Task AddAsync(T item)
        {
            await _repository.AddAsync(item);
            ClearCache();
        }

        public async Task SaveAsync(T item)
        {
            await _repository.SaveAsync(item);
            ClearCache();
        }

        public async Task RemoveAsync(T item)
        {
            await _repository.RemoveAsync(item);
            ClearCache();
        }
        public async Task Reset()
        {
            await _repository.Reset();
            ClearCache();
        }
        private void ClearCache()
        {
            if (_cacheKey is not null)
            {
                _cache?.Remove(_cacheKey);
            }
        }
    }
}
