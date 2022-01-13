using System.Threading.Tasks;
using BlazingShop.Client.Authentication.Models;
using BlazingShop.Server.DataBase.Operations.TokenService;
using Microsoft.AspNetCore.Mvc;

namespace BlazingShop.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [Route("/token")]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] AuthenticationUserModel model)
        {
            if (await _tokenService.IsValidUsernameAndPassword(model.Email, model.Password))
            {
                return Ok(await _tokenService.GenerateToken(model.Email));
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
