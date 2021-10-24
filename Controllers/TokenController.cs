using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Model;
using System;
using System.Threading.Tasks;


namespace ShopAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {

        readonly ITokenService tokenService;
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _loginManager;
        public TokenController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService
            )
        {
            _userManager = userManager;
            _loginManager = signInManager;
            this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult > RefreshAsync(TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
            {
                return BadRequest(" -RefreshAsync- Invalid client request");
            }
            string accessToken = tokenApiModel.access_token;
            string refreshToken = tokenApiModel.refresh_token;
            var principal = tokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default
                                                    // var user = userContext.LoginModels.SingleOrDefault(u => u.UserName == username);
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("-RefreshAsync- Invalid client request");
            }
            var newAccessToken = tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            var rezult = await _userManager.UpdateAsync(user);
            // userContext.SaveChanges();
            if (rezult.Succeeded)
            {
                /* 02.10.21
                 * return new ObjectResult(new
                {
                    accessToken = newAccessToken,
                    refreshToken = newRefreshToken
                });*/
                var _rezult = new TokenApiModel { access_token = newAccessToken, refresh_token = newRefreshToken };
                return Ok( _rezult);
            }
            return BadRequest(" -RefreshAsync- Invalid client request");
        }
        [HttpPost, Authorize]
        [Route("revoke")]
        public async Task<IActionResult> RevokeAsync()
        {
            string username = null;
            if (User.Identity.IsAuthenticated)
            {
                username = User.Identity.Name;
                Console.WriteLine("-RevokeAsync-"+username);
            }

            //Console.WriteLine("-RevokeAsync-"+username);
            if (username == null) return BadRequest("username==null");
            // var user = userContext.LoginModels.SingleOrDefault(u => u.UserName == username);
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return BadRequest();
            user.RefreshToken = null;
            //  userContext.SaveChanges();
            await _userManager.UpdateAsync(user);
            return NoContent();
        }
    }
}