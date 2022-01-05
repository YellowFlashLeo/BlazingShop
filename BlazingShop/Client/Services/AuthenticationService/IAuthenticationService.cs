using System.Threading.Tasks;
using BlazingShop.Client.Authentication.Models;

namespace BlazingShop.Client.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<AuthenticatedUserModel> Login(AuthenticationUserModel userToBeAuthenticated);
        Task Logout();
    }
}
