using System.Threading.Tasks;
using BlazingShop.Server.DataBase;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazingShop.Server.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class TokenController : ControllerBase
    //{
    //    private readonly DataContext _context;
    //    private readonly UserManager<IdentityUser> _userManager;

    //    public TokenController(DataContext context, UserManager<IdentityUser> userManager)
    //    {
    //        _context = context;
    //        _userManager = userManager;
    //    }

    //    [Route("/token")]
    //    [HttpPost]
    //    public async Task<ActionResult> Create(string username, string password, string grant_type)
    //    {
    //        if (await IsValidUsernameAndPassword(username, password))
    //        {
    //            return Ok();
    //        }
    //        else
    //        {
    //            return BadRequest();
    //        }
    //    }

    //    private async Task<bool> IsValidUsernameAndPassword(string username, string password)
    //    {
    //        var user = await _userManager.FindByEmailAsync(username);
    //        return await _userManager.CheckPasswordAsync(user, password);
    //    }

    //    private async Task<dynamic> GenerateToken(string username)
    //    {
    //        var user = await _userManager.FindByEmailAsync(username);
    //        //var roles = from ur in _context.
    //    }
    //}
}
