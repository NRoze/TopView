using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopView.Core.Data;
using TopView.Core.Models;

namespace TopView.Core.Services
{
    public class DataRepository : IDataRepository
    {
        private readonly AppDbContext _db;

        public DataRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<BalancePoint>> GetBalancePointsAsync() => await _db.GetBalancePointsAsync();
        public async Task AddBalancePointAsync(BalancePoint bp) => await _db.AddBalancePointAsync(bp);
        public async Task RemoveBalancePointAsync(BalancePoint bp) => await _db.DeleteBalancePointAsync(bp);
        public async Task SaveBalancePointAsync(BalancePoint bp) => await _db.SaveBalancePointAsync(bp);
    }
}
