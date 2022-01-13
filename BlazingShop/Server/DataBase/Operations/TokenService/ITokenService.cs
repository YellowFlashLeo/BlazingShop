using System.Threading.Tasks;

namespace BlazingShop.Server.DataBase.Operations.TokenService
{
   public interface ITokenService
   {
       Task<bool> IsValidUsernameAndPassword(string username, string password);
       Task<dynamic> GenerateToken(string username);
   }
}
