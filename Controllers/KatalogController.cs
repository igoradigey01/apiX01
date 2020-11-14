using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ShopDbLib.Models;
using WebShopAPI.Model; 
using Microsoft.AspNetCore.Authorization;
namespace WebShopAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KatalogController : ControllerBase
    {
        private readonly KatalogRepository _repository;
        public KatalogController(KatalogRepository repository)
        {
            _repository = repository;

        }
        [HttpGet]        
        public IEnumerable<Katalog> Katalogs()
        {
            return _repository.GetKatalogs().ToList();
            //  throw new Exception("NOt Implimetn Exception");
        }

    }





}