using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlazingShop.Shared.Modals;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BlazingShop.Server.DataBase.Operations.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _dataContext;

        public TokenService(DataContext dataContext, UserManager<User> userManager)
        {
            _userManager = userManager;
            _dataContext = dataContext;
        }

        public async Task<bool> IsValidUsernameAndPassword(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);
            return user.Password == password ? true : false;
        }

        public async Task<dynamic> GenerateToken(string username)
        {
            var user = await _userManager.FindByEmailAsync(username);
            var roles = from ur in _dataContext.UserRoles
                join r in _dataContext.Roles on ur.RoleId equals r.Id
                where ur.UserId == user.Id
                select new { ur.UserId, ur.RoleId, r.Name };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Nbf,
                    new DateTimeOffset(DateTime.Now)
                        .ToUnixTimeSeconds()
                        .ToString()),
                new Claim(JwtRegisteredClaimNames.Exp,
                    new DateTimeOffset(DateTime.Now.AddHours(2))
                        .ToUnixTimeSeconds()
                        .ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var token = new JwtSecurityToken(
                new JwtHeader(
                    new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKeyIsSecret")),
                        SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            var output = new
            {
                Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserName = username
            };

            return output;
        }
    }
}
