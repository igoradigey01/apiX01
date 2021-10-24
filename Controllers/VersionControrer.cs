using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Model;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
namespace ShopAPI.Controllers
{
    public class VetsionInfo
    {
        public string V { get; set; }
        public string Description { get; set; }
    }

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class VersionController : ControllerBase
    {
       
        string _vertion=" v0.4.8-beta-(15.09.21)--debug  vertion-- aspnetcore -net5.0";
        string _description="Create Identity net core model";


        public VersionController(){
         // _repository=repository;

        }
        
        

        [HttpGet]     
        public VetsionInfo Info()
        {
            return  new VetsionInfo{V=_vertion,Description=_description}; // отправка в формате json  (-error parsing angular response)

        }

        [HttpGet] 
        [Authorize]       
        public IActionResult Secret()
        {
            return  Ok("Secret"); // отправка в формате json  (-error parsing angular response)

        }


       
            
           



            
        


    }
    
}