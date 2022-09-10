using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Model;

using System;
using System.Threading.Tasks;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _loginManager;
        private readonly IMapper _mapper;

        public ProfileController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IMapper mapper
                       // IEmailSender emailSender
                       )
        {
            _userManager = userManager;
            _loginManager = signInManager;
            _mapper = mapper;
            // _emailSender = emailSender;

        }

       
        
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {


            var user = await _userManager.GetUserAsync(HttpContext.User);

            Console.WriteLine("test Profile"+user);

             // Console.WriteLine(""+this.HttpContext.ToString());
             // id  должен быть первым в cla
             //FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
             if (user != null)
             {
              //   var idUser = Guid.Parse(idUserClaim.Value);
                // var user = await this._userManager.FindByIdAsync(userId.Claims[]);                                     //_repository.GetUserId(idUser);
                 UserProfileDto userSerialize = new UserProfileDto();
                 userSerialize.FirstName = user.FirstName;
                userSerialize.LastName = user.LastName;
                 userSerialize.Address = user.Address;
                 userSerialize.Email = user.Email;
                 userSerialize.Phone = user.Phone;
               

                 return Ok(userSerialize);
             }
             else
             {
                 ModelState.AddModelError("User", "Данный Пользоватль Несуществует - обратитесь к Администратору ресурса");
                 Console.WriteLine("User Profile get httpGet BadRequst");
                 return BadRequest(ModelState);
             }
           
        }

        //---------------------------------------------------------------------------------

        
       
        [HttpPost("EditUser")]
        public async Task<IActionResult> UpdateUser(UserProfileDto userSerialize)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user!=null)
            {
                user.LastName = userSerialize.LastName;
                user.FirstName = userSerialize.FirstName;
                user.PhoneNumber = userSerialize.Phone;
                user.Email = userSerialize.Email;
                user.Address = userSerialize.Address;
            var result =    await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    ModelState.AddModelError("", "Что-то пошло не так");
                    return BadRequest(ModelState);
                }
               

            }             
               else
               {
                   ModelState.AddModelError("User", "Данный Пользоватль Несуществует - обратитесь к Администратору ресурса");
                   Console.WriteLine("User Profile get httpGet BadRequst");
                   return BadRequest(ModelState);
               }
           


        }

        //----------------------------------
        [Route("Delete")]        
        [HttpDelete]
        public IActionResult DeleteUser()
        {
            throw new NotImplementedException();
        }

        ///-----------------------------------------
    }
}
