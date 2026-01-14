using Microsoft.Extensions.Caching.Hybrid;
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
        public DataRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<BalancePoint>?> GetBalancePointsAsync()
        {
            return await _db.GetBalancePointsAsync().ConfigureAwait(false);
        }
        public async Task AddBalancePointAsync(BalancePoint bp) => await _db.AddBalancePointAsync(bp);
        public async Task RemoveBalancePointAsync(BalancePoint bp) => await _db.DeleteBalancePointAsync(bp);
        public async Task SaveBalancePointAsync(BalancePoint bp) => await _db.SaveBalancePointAsync(bp);
    }
}
