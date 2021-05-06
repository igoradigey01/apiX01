using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebShopAPI.Model;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
namespace WebShopAPI.Controllers
{    
        
    [ApiController]
    [Route("api/[controller]")]
    public class VersionController : ControllerBase
    {
        

 
        //-----------------------------
        string[] _l=new[]{"один","два","Три-test"};

        string _vertion=" v0.4.1-beta-(6.05.21)--debug vertion-- aspnetcore -net5.0";
        string _description="";


        public VersionController(){
         // _repository=repository;

        }
        
        

         [HttpGet]        
        public VetsionInfo Get()
        {
            return  new VetsionInfo{V=_vertion,Description=_description}; // отправка в формате json  (-error parsing angular response)
            //  throw new Exception("NOt Implimetn Exception");
        }

       
            
           



            
        


    }
    
}