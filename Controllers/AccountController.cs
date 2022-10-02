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

using AutoMapper;
using Microsoft.AspNetCore.WebUtilities;
using NETCore.MailKit.Core;
using EmailService;
using X01Api.Models.ViewModel;
using System.Security.Cryptography;
using Google.Apis.Auth;
using System.IO;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ShopAPI.Controllers
{

    [ApiController]
    [Authorize]
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
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginInputModelDto login) //[FromBody] LoginInputModel login
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
        [HttpPost("TelegramExternalLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> TelegramExternalLogin([FromBody] UserTelegramDto user)
        {

            //throw new Exception("not Empliment - ExternalLogin");

            // place bot token of your bot here
            //https://xl-01.ru/telelram-callback?id=5384361370&first_name=Igor&username=igor_01ts&auth_date=1663303742&hash=bd734108e0d695bd89a6eaf56e50b3cdc2a5adc8dc4e98a8c777bc66545bebbf
            //https://gist.github.com/anonymous/6516521b1fb3b464534fbc30ea3573c2
            // https://codex.so/telegram-auth?ysclid=l89qryxyrb334407760




            StringBuilder dataStringBuilder = new StringBuilder(256);

            dataStringBuilder.Append("auth_date");
            dataStringBuilder.Append('=');
            dataStringBuilder.Append(user.AuthDate);
            dataStringBuilder.Append('\n');

            dataStringBuilder.Append("first_name");
            dataStringBuilder.Append('=');
            dataStringBuilder.Append(user.FirstName);
            dataStringBuilder.Append('\n');

            dataStringBuilder.Append("id");
            dataStringBuilder.Append('=');
            dataStringBuilder.Append(user.Id);
            dataStringBuilder.Append('\n');


            


            dataStringBuilder.Append("username");
            dataStringBuilder.Append('=');
            dataStringBuilder.Append(user.UserName);
       //     dataStringBuilder.Append('\n');

            
            // byte[] signature = _hmac.ComputeHash(Encoding.UTF8.GetBytes(dataStringBuilder.ToString()));

            var secretKey = ShaHash(Environment.GetEnvironmentVariable("Telegram_token_bot"));

            var myHash = HashHmac(secretKey, Encoding.UTF8.GetBytes(dataStringBuilder.ToString()));

            //php   hash   https://php.ru/manual/function.hash.html?ysclid=l87m3tbcl2736101629
            //php   hash_hmac   https://www.php.net/manual/ru/function.hash-hmac.php
            //php    time()  https://www.php.net/manual/ru/function.time.php

            var teltime = int.Parse(user.AuthDate);
            var time = DateTimeOffset.Now.ToUnixTimeSeconds(); 


              if ((time - teltime) > 86400)// 24часа
            {
               return BadRequest("Data is outdated");
            }

            var myHashStr = String.Concat(myHash.Select(i => i.ToString("x2")));
            var providerKey = user.Id;
            if (myHashStr == user.Hash)
            {
                //AspNetUserLogins   table in identity BD
                //ProviderKey — это уникальный ключ Facebook, связанный с пользователем на Facebook.
               
               
                var info = new UserLoginInfo("Telegram", providerKey, "Telegram");

                var user_tel = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                
                    
                    if (user_tel == null)
                    {
                    user_tel = new User { FirstName=user.FirstName, UserName =user.UserName,NormalizedUserName= user.Id};
                        await _userManager.CreateAsync(user_tel);
                        //prepare and send an email for the email confirmation
                        await _userManager.AddToRoleAsync(user_tel, Role.Shopper);
                        await _userManager.AddLoginAsync(user_tel, info);
                    }
                    else
                    {
                        await _userManager.AddLoginAsync(user_tel, info);
                    }
                if (user_tel == null)
                    return BadRequest("Invalid External Authentication.");

                //----------------------------------------------
                var accessToken = GenerateTokenAsync(user_tel).Result;
                var refreshToken = "";

                return Ok(new TokenModelDto { access_token = accessToken, refresh_token = refreshToken });
            }


            return BadRequest("hash error");


        }

        private  byte[] ShaHash(String value) { 
            using (var hasher = SHA256.Create()) 
            
            { return hasher.ComputeHash(Encoding.UTF8.GetBytes(value)); } 
        }

        private  byte[] HashHmac(byte[] key, byte[] message)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(message);
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

        [HttpPost("GoogleExternalLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleExternalLogin ([FromBody] ExternalAuthDto externalAuth)
        {
            var payload = await VerifyGoogleToken(externalAuth);
            if (payload == null)
                return BadRequest("Invalid External Authentication.");

            var info = new UserLoginInfo(externalAuth.Provider, payload.Subject, externalAuth.Provider);

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new User { Email = payload.Email, UserName = payload.Email ,FirstName=user.FirstName,LastName=user.LastName};
                    await _userManager.CreateAsync(user);

                    //prepare and send an email for the email confirmation

                    await _userManager.AddToRoleAsync(user, Role.Shopper);
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

            var accessToken = GenerateTokenAsync(user).Result;
            var refreshToken = "";

            return Ok(new TokenModelDto { access_token = accessToken, refresh_token = refreshToken });
        }

        [HttpPost("VKExternalLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> VKExternalLogin([FromBody] ExternalAuthDto externalAuth)
        {


            var payload = VerifyVKToken(externalAuth);
            if (payload == null)
                return BadRequest("Invalid External Authentication.");

            var info = new UserLoginInfo(externalAuth.Provider, payload.UserId.ToString(), externalAuth.Provider);

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
              //  user = await _userManager.FindByEmailAsync(payload.Email);
               
                    user = new User { UserName = payload.UserId.ToString(), NormalizedUserName = payload.UserId.ToString(), FirstName = payload.FirstName, LastName = payload.LastName };
                    await _userManager.CreateAsync(user);

                    //prepare and send an email for the email confirmation

                    await _userManager.AddToRoleAsync(user, Role.Shopper);
                    await _userManager.AddLoginAsync(user, info);
                
                
            }
            else
            {
                await _userManager.AddLoginAsync(user, info);
            }

            if (user == null)
                return BadRequest("Invalid External Authentication.");

            //check for the Locked out account

            var accessToken = GenerateTokenAsync(user).Result;
            var refreshToken = "";

            return Ok(new TokenModelDto { access_token = accessToken, refresh_token = refreshToken });
        }



        //[Route("Register")]
        [HttpPost("Registration")]
        [AllowAnonymous]
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

            await _userManager.AddToRoleAsync(user, Role.Shopper);  //'shopper'

            return StatusCode(201);
        }

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
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

            var message = new Message(new string[] { forgotPasswordDto.Email }, "сбросить  пароль ", callback, null);
            await _emailSender.SendEmailAsync(message);

            return Ok();
        }


        [HttpPost("ResetPasswordMail")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordMiall([FromBody] ResetPasswordMailDto resetPasswordDto)
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
        [AllowAnonymous]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            //https://github.com/CodeMazeBlog/angular-identity-aspnetcore-security/tree/angular-google-authentication-identity
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


                 };
            foreach (var r in userRoles)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, r));

            }

            //var mySecret = Environment.GetEnvironmentVariable("ClientSecrets"); // ключ для шифрации
            var mySecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("ClientSecrets"))
                );

            int LIFETIME = 240;//1; //60; // время жизни токена 

            var tokenHandler = new JwtSecurityTokenHandler();
            var time = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), //Claims

                Expires = time.Add(TimeSpan.FromMinutes(LIFETIME)),
                Issuer = Environment.GetEnvironmentVariable("Issuer"),// издатель токена
                Audience = "http://localhost:8080,http://localhost:4200,x-01.ru,xf-01.ru,xl-01.ru",// Environment.GetEnvironmentVariable("Audience"),// потребитель токена
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


        private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDto externalAuth)
        {

            //https://console.cloud.google.com/apis/credentials?project=x-01-mystore
            //https://github.com/CodeMazeBlog/angular-identity-aspnetcore-security/blob/angular-google-authentication-identity/Web%20API/CompanyEmployees/CompanyEmployees/Program.cs
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { Environment.GetEnvironmentVariable("Google_idToken") }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);

                Console.WriteLine("GoogleJsonWebSignature.ValidateAsync--"+payload);
                return payload;
            }
            catch (Exception ex)
            {
                //log an exception
                return null;
            }
        }

        //https://www.kbss.ru/blog/netcore/325.html
        private VkProfileDto VerifyVKToken(ExternalAuthDto externalAuth)
        {
            // https://vk.com/dev/client_cred_flow
            //https://ru.stackoverflow.com/questions/590065/vk-api-%D0%9A%D0%B0%D0%BA-%D0%B8%D0%B7-standalone-%D0%BF%D1%80%D0%B8%D0%BB%D0%BE%D0%B6%D0%B5%D0%BD%D0%B8%D1%8F-%D0%BF%D1%80%D0%BE%D0%B2%D0%B5%D1%80%D0%B8%D1%82%D1%8C-%D0%B2%D0%B0%D0%BB%D0%B8%D0%B4%D0%BD%D0%BE%D1%81%D1%82%D1%8C-%D1%82%D0%BE%D0%BA%D0%B5%D0%BD%D0%B0-%D0%BC%D0%B5%D1%82%D0%BE%D0%B4%D0%BE%D0%BC-secure-c?ysclid=l8k6p5f1iy824577111
            VkProfileDto profile = new VkProfileDto();

           

            string str = GetRequest("api.vk.com", "https://api.vk.com:443/method/users.get?user_ids="
                + externalAuth.IdUser + "&v=5.131"                
                + "&access_token=" + externalAuth.IdToken);
            dynamic stuff = JsonConvert.DeserializeObject(str);
            
            try
            {
                Console.WriteLine("VerifyVKToken---"+stuff);
                profile.FirstName = stuff.response[0].first_name;
                profile.LastName = stuff.response[0].last_name;
                profile.UserId = stuff.response[0].id;

            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return null; 
            }

           

            return profile;

        }


       
        /// <summary>
        /// Отправляем запрос на получение 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private  string GetRequest(string host, string req)
        {
            string str = "";

            var Vk = new HttpClient();
            Vk.DefaultRequestHeaders.Add("Connection", "close");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(req);
            request.UseDefaultCredentials = true;
            request.PreAuthenticate = true;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = "GET";
            request.Host = host;
           // request.UserAgent = "RM";
            request.ContentType = "application/x-www-form-urlencoded";
            request.KeepAlive = false;

            using (HttpWebResponse responsevk = (HttpWebResponse)request.GetResponse())
            using (var stream = responsevk.GetResponseStream())
            using (var streamReader = new StreamReader(stream, Encoding.UTF8))
            {
                str = streamReader.ReadToEnd();
            }
            return str;
        }

    }
}

