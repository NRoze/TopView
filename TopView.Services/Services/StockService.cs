using System.Net.Http;
using System.Net.Http.Json;
using TopView.Model.Models;
using TopView.Services.Interfaces;

namespace TopView.Services
{
    public class StockService : IStockService
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;

        public StockService(string token, HttpClient? httpClient = null)
        {
            _httpClient = httpClient ?? new HttpClient();
            _token = token;
        }

        /// <summary>
        /// Fetches stock quote for any symbol.
        /// </summary>
        /// <param name="symbol">Ticker symbol, e.g. "AAPL"</param>
        /// <returns>Quote object or null if failed</returns>
        public async Task<Quote?> GetQuoteAsync(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException("Symbol cannot be empty.", nameof(symbol));

            string url = $"https://finnhub.io/api/v1/quote?symbol={symbol}&token={_token}";

            try
            {
                return await _httpClient.GetFromJsonAsync<Quote>(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching quote for {symbol}: {ex.Message}");
                return null;
            }
        }
    }
}