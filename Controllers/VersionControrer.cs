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
        public string Version { get; set; }
        public string Description { get; set; }
    }

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class VersionController : ControllerBase
    {
       
        string _version="b2.9.22";
        string _description= "Api shop- вторая редакция ( aspnetcore -net5.0)(01.10.22)";


        public VersionController(){
         // _repository=repository;

        }

      
        
        

        [HttpGet]     
        public VetsionInfo Info()
        {
            return  new VetsionInfo{Version=_version,Description=_description}; // отправка в формате json  (-error parsing angular response)

        }

         

    }
    
}