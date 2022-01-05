using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BlazingShop.Server.DataBase.Operations.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DataContext _dataContext;

        public TokenService(DataContext dataContext, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _dataContext = dataContext;
        }

    }
}
