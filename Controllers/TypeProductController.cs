using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebShopAPI.Model;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using ShopDb;

namespace WebShopAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TypeProductController : ControllerBase
    {
        private readonly TypeProductRepository _repository;

        public TypeProductController(
             TypeProductRepository repository
        )
        {
            _repository = repository;

        }


        [HttpGet]
        public async Task<IEnumerable<TypeProduct>> Get()
        {
            // return "test-Get_TypeProductController";
            //  test_MySql();
            // throw new Exception("NOt Implimetn Exception");
            return await _repository.Get();

        }
    }
}