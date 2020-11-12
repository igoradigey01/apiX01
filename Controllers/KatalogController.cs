using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DbClassLib.Models;
using AuthApi.Model; 
using Microsoft.AspNetCore.Authorization;
namespace AuthApi.Controllers
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
        [Route("Type")]
        public IEnumerable<Katalog> ProductTypes()
        {
            return _repository.GetKatalogs().ToList();
            //  throw new Exception("NOt Implimetn Exception");
        }

    }





}