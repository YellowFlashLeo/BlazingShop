using System.Threading.Tasks;

namespace BlazingShop.Server.DataBase.Operations.StatsServiceDB
{
   public interface IStatsService
   {
       Task<int> GetVisits();
       Task IncrementVisits();
   }
}
