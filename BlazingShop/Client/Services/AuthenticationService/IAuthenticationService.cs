using System.Threading.Tasks;
using BlazingShop.Client.Authentication.Models;
using BlazingShop.Shared.Modals;

namespace BlazingShop.Client.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<AuthenticatedUserModel> Login(AuthenticationUserModel userToBeAuthenticated);
        Task RegisterUser(CreateUserModel model);
        Task Logout();
    }
}
