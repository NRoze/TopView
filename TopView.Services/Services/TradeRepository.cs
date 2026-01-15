using TopView.Common.Interfaces;
using TopView.Model.Data;
using TopView.Model.Models;

namespace TopView.Services
{
    public class TradeRepository : IRepository<Trade>
    {
        private readonly AppDbContext _db;

        public TradeRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<Trade>?> GetAllAsync() => await _db.GetTradesAsync();
        public async Task AddAsync(Trade trade) => await _db.AddTradeAsync(trade);
        public async Task SaveAsync(Trade trade) => await _db.SaveTradeAsync(trade);
        public async Task RemoveAsync(Trade trade) => await _db.DeleteTradeAsync(trade);
        public async Task Reset() => await _db.Reset();
    }
}
