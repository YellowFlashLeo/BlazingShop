using System;
using System.Threading.Tasks;
using BlazingShop.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazingShop.Server.DataBase.Operations.StatsServiceDB
{
    public class StatsService : IStatsService
    {
        private readonly DataContext _dbContext;
        public StatsService(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> GetVisits()
        {
            var stats = await _dbContext.Stats.FirstOrDefaultAsync();
            if (stats == null)
                return 0;
            return stats.Visits;
        }

        public async Task IncrementVisits()
        {
            var stats = await _dbContext.Stats.FirstOrDefaultAsync();
            if (stats == null)
            {
                _dbContext.Stats.Add(new Stats
                {
                    Visits = 1,
                    LastVisit = DateTime.Now
                });
            }
            else
            {
                stats.Visits++;
                stats.LastVisit = DateTime.Now;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
