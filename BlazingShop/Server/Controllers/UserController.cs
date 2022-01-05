using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlazingShop.Server.DataBase;
using BlazingShop.Shared.DTOs;
using BlazingShop.Shared.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BlazingShop.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public UserController(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public User GetById()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = new User();
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("User/Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            List<ApplicationUserModel> output = new List<ApplicationUserModel>();

            var users = _context.Users.ToList();
            var userRoles = from ur in _context.UserRoles
                            join r in _context.Roles on ur.RoleId equals r.Id
                            select new { ur.UserId, ur.RoleId, r.Name };

            foreach (var user in users)
            {
                ApplicationUserModel u = new ApplicationUserModel
                {
                    Id = user.Id,
                    Email = user.EmailAddress
                };

                // get correct roleId roleName pair for each user
                u.Roles = userRoles.Where(x => x.UserId == u.Id)
                    .ToDictionary(key => key.RoleId, val => val.Name);
                output.Add(u);
            }

            return output;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("User/Admin/GetAllRoles")]
        public Dictionary<string, string> GetAllRoles()
        {
            var roles = _context.Roles.ToDictionary(key => key.Id, val => val.Name);

            return roles;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("User/Admin/AddRole")]
        public async Task AddRole(UserRolePairModel pairing)
        {
            var user = await _userManager.FindByIdAsync(pairing.UserId);
            await _userManager.AddToRoleAsync(user, pairing.RoleName);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("User/Admin/RemoveRole")]
        public async Task RemoveRole(UserRolePairModel pairing)
        {
            var user = await _userManager.FindByIdAsync(pairing.UserId);
            await _userManager.RemoveFromRoleAsync(user, pairing.RoleName);
        }
    }
}
