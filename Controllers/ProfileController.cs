using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Model;
using ShopAPI.Models;
using System;
using System.Threading.Tasks;

namespace X01Api.Controllers
{
    [Route("api/[controller]")]
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

        //  [Route("Profile")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            /* //int i=this.HttpContext.Request.Headers.Count;

             *//*
             foreach (var item in this.HttpContext.Request.Headers)
             {
                  Console.WriteLine(item.Value);

             }
             *//*


             // Console.WriteLine(this.HttpContext.Request.Headers.ToArray());
             var guidClaim = this.HttpContext.User.Claims.FirstOrDefault();

             // Console.WriteLine("test Profile");

             // Console.WriteLine(""+this.HttpContext.ToString());
             // id  должен быть первым в cla
             //FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
             if (guidClaim != null)
             {
              //   var idUser = Guid.Parse(idUserClaim.Value);
                 var user = await this._userManager.FindByIdAsync(guidClaim.Value);                                     //_repository.GetUserId(idUser);
                 UserProfileDto userSerialize = new UserProfileDto();
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
             }*/
            throw new NotImplementedException();
        }

        //---------------------------------------------------------------------------------

        // [Route("Edit")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserProfileDto userSerialize)
        {
            Console.WriteLine("SetUserProfile-----------Start--");

            /*   var guidClaim = this.HttpContext.User.Claims.FirstOrDefault();


               if (guidClaim != null)
               {
                  // int idUser = int.Parse(guidClaim.Value);
                   var user = await this._userManager.FindByIdAsync(guidClaim.Value)                                // _repository.GetUserId(idUser);

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
               }*/
            throw new NotImplementedException();


        }

        //----------------------------------
        [Route("Delete")]
        [Authorize(Roles = Role.Admin)]
        [HttpDelete]
        public IActionResult DeleteUser()
        {
            throw new NotImplementedException();
        }

        ///-----------------------------------------
    }
}
