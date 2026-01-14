using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopView.Model.Data;
using TopView.Model.Models;
using TopView.Services.Interfaces;

namespace TopView.Services
{
    public class TradeRepository : ITradeRepository
    {
        private readonly AppDbContext _db;

        public TradeRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task AddAsync(Trade account) => await _db.AddTradeAsync(account);
        public async Task SaveAsync(Trade account) => await _db.SaveTradeAsync(account);
        public async Task<List<Trade>> GetTradesAsync() => await _db.GetTradesAsync();

        public async Task RemoveAsync(Trade account) => await _db.DeleteTradeAsync(account);
    }
}
