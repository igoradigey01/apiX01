using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebShopAPI.Model;
using Microsoft.AspNetCore.Authorization;
namespace WebShopAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VersionController : ControllerBase
    {
        

 
        //-----------------------------
        string[] _l=new[]{"один","два","Три-test"};

        string _vertionName="Test Katalog v1(12.11.20)";
        public VersionController(){
         // _repository=repository;

        }
        
        
         [HttpGet]        
        public String Get()
            {
                
         //  test_MySql();
        return _vertionName;
            
        }

       
            
           



            
        


    }
    
}