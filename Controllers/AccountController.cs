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

namespace ShopAPI.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        public readonly UserManager<AppUser> _userManager;
        public readonly SignInManager<AppUser> _loginManager;
        private readonly AuthRepository _repository;


        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager
                       )
        {
            _userManager = userManager;
            _loginManager = signInManager;

        }



        //////////-------------------------------Контроллеры-------------------

        [Route("Login")]
        // [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> PassLogInAsync([FromBody] LoginInputModel login)
        {

            //  Console.WriteLine("PassLogInAsync ----",login.Email);

            if (login == null)
            {
                return BadRequest("PassLogIn-- Invalid client request");
            }

            // var user = await _userManager.FindByEmailAsync(model.UserName); //??? 
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                ModelState.AddModelError("username", "пользователь не найден!");
            }
            var result = await _loginManager.PasswordSignInAsync(user, login.Password, login.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {

                var accessToken = GenerateTokenAsync(user).Result;
                var refreshToken = "";
                //  var rest=new{access_token=token};

                //  var access_token = JsonSerializer.Serialize( token);
                /*  return Ok(new
                  {
                      access_token = accessToken,
                      refresh_token = refreshToken
                  });*/
                return Ok(new TokenApiModel { access_token = accessToken, refresh_token = refreshToken });
            }

            //     return Unauthorized(); */
            return BadRequest("ошибка что-то пошло не так ");


        }

        /// FaceBook VK Instogramm singIn 04.08.21
        [HttpPost]
        [Route("[action]")]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            throw new Exception("not Empliment - ExternalLogin");
            /* var redirectUrl = Url.Action(nameof(ExternalLoginCollback), "Auth", new { returnUrl });
            //  var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl); var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider); */
        }

        [Route("Register")]
        [HttpPost]
        public IActionResult RegisterNewUser([FromBody] UserSerialize user)
        {

            Console.WriteLine("---------------------------");
            Console.WriteLine(user.Name);

            AppUser newUser = new AppUser();  //{Role=Role.User};16.09.21
            //-----------------
            if (user.Password == null)
            {
                ModelState.AddModelError("Password", "Незадан Пароль");

            }
            newUser.Password = user.Password;

            //--------------------------------------------

            if (user.Phone == null)
            {
                ModelState.AddModelError("Phone", "Незадан Номер Телефона");
                return BadRequest(ModelState);

            }
            if (_repository.ValidatePhoneUser(user.Phone))
            {
                newUser.Phone = user.Phone;
            }
            else
            {
                ModelState.AddModelError("Phone", "Такой Номер Телефона Уже Существует");
                return BadRequest(ModelState);
            }
            //-------------------------------------------
            if (user.Email != null)
            {
                if (_repository.ValidateEmailUser(user.Email))
                {
                    newUser.Email = user.Email;
                }
                else
                {
                    ModelState.AddModelError("Email", "Такой Email Уже Существует");
                    return BadRequest(ModelState);
                }
            }
            //-----------------------------
            newUser.Name = user.Name;
            newUser.Address = user.Address;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _repository.CreateUser(newUser);

            return Ok();

            // return Ok("Regiser new User");
            /*
             // обработка частных случаев валидации
            if (user.Age == 99)
                ModelState.AddModelError("Age", "Возраст не должен быть равен 99");
 
            if (user.Name == "admin")
            {
                ModelState.AddModelError("Name", "Недопустимое имя пользователя - admin");
            }
            // если есть лшибки - возвращаем ошибку 400
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
 
            // если ошибок нет, сохраняем в базу данных
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Ok(user);
            */

            // throw new NotImplementedException();
        }

        [Route("Profile")]
        [Authorize]
        [HttpGet]
        public IActionResult GetUserProfile()
        {
            //int i=this.HttpContext.Request.Headers.Count;

            /*
            foreach (var item in this.HttpContext.Request.Headers)
            {
                 Console.WriteLine(item.Value);
                
            }
            */


            // Console.WriteLine(this.HttpContext.Request.Headers.ToArray());
            var idUserClaim = this.HttpContext.User.Claims.FirstOrDefault();

            // Console.WriteLine("test Profile");

            // Console.WriteLine(""+this.HttpContext.ToString());
            // id  должен быть первым в cla
            //FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (idUserClaim != null)
            {
                int idUser = int.Parse(idUserClaim.Value);
                var user = _repository.GetUserId(idUser);
                UserSerialize userSerialize = new UserSerialize();
                userSerialize.Name = user.Name;
                userSerialize.Address = user.Address;
                userSerialize.Email = user.Email;
                userSerialize.Phone = user.Phone;
                userSerialize.Password = user.Password;
                // Console.WriteLine("User Profile get httpGet ok");

                return Ok(userSerialize);
            }
            else
            {
                ModelState.AddModelError("User", "Данный Пользоватль Несуществует - обратитесь к Администратору ресурса");
                Console.WriteLine("User Profile get httpGet BadRequst");
                return BadRequest(ModelState);
            }
            // throw new NotImplementedException();
        }

        //---------------------------------------------------------------------------------

        [Route("Edit")]
        [Authorize]
        [HttpPost]
        public IActionResult SetUserProfile(UserSerialize userSerialize)
        {
            Console.WriteLine("SetUserProfile-----------Start--");

            var idUserClaim = this.HttpContext.User.Claims.FirstOrDefault();


            if (idUserClaim != null)
            {
                int idUser = int.Parse(idUserClaim.Value);
                var user = _repository.GetUserId(idUser);

                if (userSerialize.Name != user.Name)
                {
                    user.Name = userSerialize.Name;
                }
                if (userSerialize.Address != user.Address)
                {
                    user.Address = userSerialize.Address;
                }
                if (userSerialize.Email != user.Email)
                {
                    user.Email = userSerialize.Email;
                }
                if (userSerialize.Phone != user.Phone)
                {
                    user.Phone = userSerialize.Phone;
                }
                if (userSerialize.Password != user.Password)
                {
                    user.Password = userSerialize.Password;
                }
                // Console.WriteLine("User Profile get httpGet ok");
                _repository.SaveUser(user);
                //throw new NotImplementedException();

                return Ok();

            }
            else
            {
                ModelState.AddModelError("User", "Данный Пользоватль Несуществует - обратитесь к Администратору ресурса");
                Console.WriteLine("User Profile get httpGet BadRequst");
                return BadRequest(ModelState);
            }
            // throw new NotImplementedException();


        }

        //----------------------------------
        [Route("Delete")]
        [Authorize(Roles = Role.Admin)]
        [HttpDelete]
        public IActionResult DeleteUserProfileserProfile()
        {
            throw new NotImplementedException();
        }

        ///-------------------------------------------------------------------------------------
        [HttpGet]
        [Route("IsValid")]
        [Authorize]
        public IActionResult Get()
        {
            Console.WriteLine("isValid get ok");

            return Ok();

        }


        //////////////////////------------------Создаем Токен-----------------------------------
        private async Task<string> GenerateTokenAsync(AppUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);




            var claims = new List<Claim> {
                 new Claim(JwtRegisteredClaimNames.NameId, user.UserName) ,
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
        /////////////------------Проверяем User и Создаем утверждения для токена.----------------------------------------
        /*  private ClaimsIdentity GetUserIdentity(string email, string password)
         {
                    AppUser user = AuthUser(email,password);
                    if (user != null)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                          //  new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()) 16.09.21
                        };
                        ClaimsIdentity claimsIdentity =
                        new ClaimsIdentity(claims, "access_token", ClaimsIdentity.DefaultNameClaimType,
                            ClaimsIdentity.DefaultRoleClaimType);
                        return claimsIdentity;
                    }

                    // если пользователя не найдено
                    return null;
        } */





        //////////////////-----------------------------------------------------

        /* public bool ValidateCurrentToken(string token)
        {
            var mySecret = _authOptions.Secret;
            var mySecurityKey =_authOptions.GetSymmerySecuritiKey();

            var myIssuer = _authOptions.Issuer;
            var myAudience = _authOptions.Audience;

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = myIssuer,
                    ValidAudience = myAudience,
                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        } */



    }
}