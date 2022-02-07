using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShopAPI.Model;
using System;
using System.Collections.Generic;
//-----------------
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ShopAPI.Models;

using AutoMapper;
using Microsoft.AspNetCore.WebUtilities;
using NETCore.MailKit.Core;
using EmailService;


namespace ShopAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _loginManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;


        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IMapper mapper,
            IEmailSender emailSender
                       )
        {
            _userManager = userManager;
            _loginManager = signInManager;
            _mapper = mapper;
            _emailSender = emailSender;

        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]LoginInputModelDto login) //[FromBody] LoginInputModel login
        {

            Console.WriteLine("PassLogInAsync ----", login.Email);

            if (login == null)
            {
                return BadRequest(" Неверный запрос клиента");
            }

            // var user = await _userManager.FindByEmailAsync(model.UserName); //??? 
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                ModelState.AddModelError("username", "пользователь не найден!");
                return Unauthorized("пользователь не найден!");
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("email", "Электронная почта не подтверждена!");
                return Unauthorized("Email не подтвержден");
            }

            var result = await _loginManager.PasswordSignInAsync(user, login.Password, login.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {

                var accessToken = GenerateTokenAsync(user).Result;
                var refreshToken = "";

                return Ok(new TokenModelDto { access_token = accessToken, refresh_token = refreshToken });
            }

            // return Unauthorized();
            return Unauthorized("неверный пароль "); //неверный пароль


        }

        /// FaceBook VK Instogramm singIn 04.08.21
       /* [HttpPost("ExternalLogin")]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            throw new Exception("not Empliment - ExternalLogin");
            *//* var redirectUrl = Url.Action(nameof(ExternalLoginCollback), "Auth", new { returnUrl });
            //  var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl); var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider); *//*
        }*/

        [HttpPost("ExternalLogin")]
        public async Task<IActionResult> ExternalLogin([FromBody] ExternalAuthDto externalAuth)
        {
            throw new Exception("not implimetn exeption 30.11.21");
            
           /* var payload = await _jwtHandler.VerifyGoogleToken(externalAuth);
            if (payload == null)
                return BadRequest("Invalid External Authentication.");

            var info = new UserLoginInfo(externalAuth.Provider, payload.Subject, externalAuth.Provider);

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);

                if (user == null)
                {
                    user = new User { Email = payload.Email, UserName = payload.Email };
                    await _userManager.CreateAsync(user);

                    //prepare and send an email for the email confirmation

                    await _userManager.AddToRoleAsync(user, "Viewer");
                    await _userManager.AddLoginAsync(user, info);
                }
                else
                {
                    await _userManager.AddLoginAsync(user, info);
                }
            }

            if (user == null)
                return BadRequest("Invalid External Authentication.");

            //check for the Locked out account

            var token = await _jwtHandler.GenerateToken(user);
            return Ok(new AuthResponseDto { Token = token, IsAuthSuccessful = true });*/
        }

        //[Route("Register")]
        [HttpPost("Registration")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration == null || !ModelState.IsValid)
                return BadRequest("данные не валидны");

            var user = _mapper.Map<User>(userForRegistration);

            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(new RegistrationResponseDto { Errors = errors });
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string>
            {
                {"token", token },
                {"email", user.Email }
            };

            var callback = QueryHelpers.AddQueryString(userForRegistration.ClientURI, param);

            try
            {

                var message = new Message(new string[] { user.Email }, "Токен подтверждения электронной почты,Возможно Почта недействительна", callback, null);
                await _emailSender.SendEmailAsync(message);
            }
            catch
            {
                
                 await _userManager.DeleteAsync(user);

                return BadRequest("Токен подтверждения электронной почты неотправлен");
            }

            await _userManager.AddToRoleAsync(user,Role.Shopper);  //'shopper'

            return StatusCode(201);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
                return BadRequest("Invalid Request");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string>
            {
                {"token", token },
                {"email", forgotPasswordDto.Email }
            };

            var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param);

            var message = new Message(new string[] {forgotPasswordDto.Email }, "сбросить  пароль ", callback, null);
            await _emailSender.SendEmailAsync(message);

            return Ok();
        }


        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return BadRequest("Invalid Request");

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => e.Description);

                return BadRequest(new { Errors = errors });
            }

            return Ok();
        }


        [HttpGet("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("Invalid Email Confirmation Request");

            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
                return BadRequest("Invalid Email Confirmation Request");

            return Ok();
        }

        //--------------------------------------------
        [HttpGet("IsTokenValid")]
        [Authorize]
        public IActionResult IsTokenValid()
        {
            Console.WriteLine("isValid get ok");
            // this is sample--- return for Json
            //  return Json(isValid);
            // return Json(new { isValid = isValid.ToString() });
            // return Ok(new { isValid = isValid.ToString() });
            return Ok();

        }


        //////////////////////------------------Создаем Токен-----------------------------------
        private async Task<string> GenerateTokenAsync(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);




            var claims = new List<Claim> {
                 new Claim(JwtRegisteredClaimNames.NameId, user.Id) ,
                // new Claim(JwtRegisteredClaimNames.NameId,user.Id),
              //  new Claim(ClaimsIdentity.DefaultRoleClaimType,) // частный случай авторизации на основе claims, 
                 //так как роль(role) это тот же объект Claim, имеющий тип ClaimsIdentity.DefaultRoleClaimType
                 
                 };
            foreach (var r in userRoles)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, r));

            }

            //var mySecret = Environment.GetEnvironmentVariable("ClientSecrets"); // ключ для шифрации
            var mySecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("ClientSecrets"))
                );

            int LIFETIME = 60; // время жизни токена 

            var tokenHandler = new JwtSecurityTokenHandler();
            var time = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), //Claims

                Expires = time.Add(TimeSpan.FromMinutes(LIFETIME)),
                Issuer = Environment.GetEnvironmentVariable("Issuer"),// издатель токена
                Audience = Environment.GetEnvironmentVariable("Audience"),// потребитель токена
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }






    }
}
    
