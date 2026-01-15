using TopView.Common.Interfaces;
using TopView.Model.Data;
using TopView.Model.Models;

namespace TopView.Services
{
    public class DataRepository : IRepository<BalancePoint>
    {
        private readonly AppDbContext _db;
        public DataRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<BalancePoint>?> GetAllAsync()
        {
            return await _db.GetBalancePointsAsync().ConfigureAwait(false);
        }
        public async Task AddAsync(BalancePoint bp) => await _db.AddBalancePointAsync(bp);
        public async Task RemoveAsync(BalancePoint bp) => await _db.DeleteBalancePointAsync(bp);
        public async Task SaveAsync(BalancePoint bp) => await _db.SaveBalancePointAsync(bp);
        public async Task Reset() => await _db.Reset();

    }
}
