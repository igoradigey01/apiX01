using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebShopAPI.Model;
//-----------------
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using ShopDbLib.Models;


namespace WebShopAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
            const  string _roleAdmin="Admin";
        const string _roleUser = "User";
        const string _roleAll = "Admin,User";

       

            string[] _l=new[]{"один","два","Три-test"};
       private readonly AuthLib.AuthOptions _authOptions;
        private readonly AuthRepository _repository;
        

        private readonly ILogger<AuthController> _logger;
 //////////////////----------Конструктор----------------------------------------------------------------
         public AuthController(ILogger<AuthController> logger,
             IOptions<AuthLib.AuthOptions> options,AuthRepository repository) // конструктор
        {
            _logger = logger;
            _authOptions=options.Value;
             _repository=repository;
        }


      
//////////-------------------------------Контроллеры-------------------
       
        [Route("Login")]
        [HttpPost]
        public IActionResult LogIn([FromBody]Login login){
         ClaimsIdentity userIdentity=GetUserIdentity(login.Email,login.Password);

            if(userIdentity!=null){
                
               var toket= GenerateToken(userIdentity);
               var response = new
            {
                access_token = toket
               
            };
                
            return Ok(response);
            }
            
                return Unauthorized();
            

        }

        [Route("Register")]
         [HttpPost]
        public IActionResult RegisterNewUser([FromBody]UserSerialize user)
        {
            
          Console.WriteLine("---------------------------");
            Console.WriteLine(user.Name);
            
            User newUser=new  User {Role=Role.User};
            //-----------------
            if(user.Password ==null){
                ModelState.AddModelError("Password","Незадан Пароль");

            }
            newUser.Password=user.Password;

            //--------------------------------------------
            
            if(user.Phone==null){
                ModelState.AddModelError("Phone","Незадан Номер Телефона");
                return BadRequest(ModelState);
               
            }
            if(_repository.ValidatePhoneUser(user.Phone)){
            newUser.Phone=user.Phone;
            }
            else{
                ModelState.AddModelError("Phone","Такой Номер Телефона Уже Существует");
                return BadRequest(ModelState);
            }
            //-------------------------------------------
            if(user.Email!=null){
                if(_repository.ValidateEmailUser(user.Email)){
                    newUser.Email=user.Email;
                }
                else{
                    ModelState.AddModelError("Email","Такой Email Уже Существует");
                    return BadRequest(ModelState);
                }
            }
            //-----------------------------
            newUser.Name=user.Name;
            newUser.Address=user.Address;
            if(!ModelState.IsValid){
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
          var idUserClaim=   this.HttpContext.User.Claims.FirstOrDefault(); 
          
         // Console.WriteLine("test Profile");

         // Console.WriteLine(""+this.HttpContext.ToString());
                 // id  должен быть первым в cla
           //FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
          if(idUserClaim!=null){
          int idUser=int.Parse(idUserClaim.Value);
         var user=  _repository.GetUserId(idUser);
         UserSerialize userSerialize=new UserSerialize();
         userSerialize.Name=user.Name;
         userSerialize.Address=user.Address;
         userSerialize.Email=user.Email;
         userSerialize.Phone=user.Phone;
         userSerialize.Password=user.Password;
        // Console.WriteLine("User Profile get httpGet ok");

         return Ok(userSerialize);
          }
          else{
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
                      
          var idUserClaim=   this.HttpContext.User.Claims.FirstOrDefault(); 
          
         
          if(idUserClaim!=null){
          int idUser=int.Parse(idUserClaim.Value);
         var user=  _repository.GetUserId(idUser);
         
         if(userSerialize.Name!=user.Name)
         {
            user.Name=userSerialize.Name;
         }
         if( userSerialize.Address!=user.Address)
         {
         user.Address=userSerialize.Address;
         }
         if( userSerialize.Email!=user.Email)
         {
         user.Email=userSerialize.Email;
         }
         if(userSerialize.Phone!=user.Phone)
         {
         user.Phone=userSerialize.Phone;
         }
         if(userSerialize.Password!=user.Password)
         {
         user.Password=userSerialize.Password;
         }
        // Console.WriteLine("User Profile get httpGet ok");
        _repository.SaveUser(user);
        //throw new NotImplementedException();

         return Ok();
         
          }
          else{
               ModelState.AddModelError("User", "Данный Пользоватль Несуществует - обратитесь к Администратору ресурса");
               Console.WriteLine("User Profile get httpGet BadRequst");
              return BadRequest(ModelState);
          }
           // throw new NotImplementedException();


        }
       
       //----------------------------------
        [Route("Delete")]
        [Authorize(Roles = _roleUser)]
        [HttpDelete]
        public IActionResult DeleteUserProfileserProfile()
        {
            throw new NotImplementedException();
        }

        ///-------------------------------------------------------------------------------------
        [HttpGet]
        [Route("IsValid")]
        [Authorize]        
        public IActionResult Get(){       
            Console.WriteLine("isValid get ok"); 
           
            return Ok();

        }
////////// -----------------------------------------
    private User AuthUser(string email,string password){

           // var user =_users.SingleOrDefault(u=> u.Password==password && u.Email==email);
           var user=_repository.ValidateUser(email,password);
            return user;
        }

//////////////////////------------------Создаем Токен-----------------------------------
    private string GenerateToken(ClaimsIdentity userIdentity)
{
	var mySecret = _authOptions.Secret; // ключ для шифрации
	var mySecurityKey =_authOptions.GetSymmerySecuritiKey();
	var myIssuer = _authOptions.Issuer;  // издатель токена
	var myAudience = _authOptions.Audience;// потребитель токена
    int LIFETIME = _authOptions.TokenLifeTime ; // время жизни токена 

	var tokenHandler = new JwtSecurityTokenHandler();
    var time = DateTime.UtcNow;
	var tokenDescriptor = new SecurityTokenDescriptor
	{

         // 2. Создаем утверждения для токена.
        
        
		Subject = userIdentity,
        
		Expires = time.Add(TimeSpan.FromMinutes(LIFETIME)),
		Issuer = myIssuer,
		Audience = myAudience,
		SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
	};

	var token = tokenHandler.CreateToken(tokenDescriptor);
	return tokenHandler.WriteToken(token);
}
/////////////------------Проверяем User и Создаем утверждения для токена.----------------------------------------
 private ClaimsIdentity GetUserIdentity(string email, string password)
 {
            User user = AuthUser(email,password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "access_token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
 
            // если пользователя не найдено
            return null;
}
    




//////////////////-----------------------------------------------------

public bool ValidateCurrentToken(string token)
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
}


       
    }
}