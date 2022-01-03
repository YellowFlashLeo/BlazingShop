using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace BlazingShop.Client.Services.StatsService
{
    public class StatsService : IStatsService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        public StatsService(HttpClient http,ILocalStorageService localStorage)
        {
            _httpClient = http;
            _localStorage = localStorage;
        }
        public async Task GetVisits()
        {
            int visits =  int.Parse(await _httpClient.GetStringAsync("api/Stats"));
            Console.WriteLine($"Visits: {visits}");
        }

        public async Task IncrementVisits()
        {
            DateTime? lastVisit = await _localStorage.GetItemAsync<DateTime?>("lastVisit");
            if(lastVisit == null || ((DateTime)lastVisit).Date != DateTime.Now.Date)
            {
                await _localStorage.SetItemAsync("lastVisit", DateTime.Now);
                await _httpClient.PostAsync("api/Stats",null);
            }
        }
    }
}
