using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ShopDb;
using ShopAPI.Model;

using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
namespace ShopAPI.Controllers
{
    [ApiController]
    [Authorize(Roles = Role.Admin+","+ Role.Manager)]
   // [Authorize(Roles = Role.Manager)]
    [Route("api/[controller]/[action]")]
    public class KatalogController : ControllerBase
    {
        private readonly KatalogRepository _repository;
        public KatalogController(KatalogRepository repository)
        {
            _repository = repository;

        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<Katalog>> Get()
        {
            return await _repository.Get();
            //  throw new Exception("NOt Implimetn Exception");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<Product>> Products(int id)
        {
            return await _repository.Get(id);
            //  throw new Exception("NOt Implimetn Exception");
        }


        // api/katalog (post) создать
        [HttpPost]       
        public async Task<ActionResult<Katalog>> Post(Katalog item)
        {
            if (item == null)
            {
                return BadRequest("angular form data ==null");
            }
            // Console.WriteLine("Task< ActionResult<Katalog>> Post(Katalog item)----"+item.Name +"-"+item.Id+"-"+item.Model);
            var flag = await _repository.Add(item);
            if (flag.Flag)
            {
                return Ok(item);
            }
            else
            {
                return BadRequest(flag.Message);
            }
        }

        // PUT api/katalog/ (put) -изменить
        [HttpPut]        
        public async Task<ActionResult> Put(Katalog item)
        {

            if (item == null)
            {
                return BadRequest();
            }
            // if(id!=item.Id) return BadRequest();
            var flag = await _repository.Update(item);
            if (flag.Flag)
            {
                Katalog katalog = flag.Item as Katalog;
                Console.WriteLine(katalog.Name + "-----" + katalog.Id);
                return Ok();

            }


            return BadRequest(flag.Message);
        }

        // DELETE api/katalog/5
        [HttpDelete("{id}")]       
        public async Task<ActionResult> Delete(int id)
        {
            var flagValid = await _repository.Delete(id);
            if (!flagValid.Flag)
            {
                return BadRequest(flagValid.Message);
            }
            return Ok();
        }





    }





}