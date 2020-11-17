using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ShopDbLib.Models;
using WebShopAPI.Model; 
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
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
        public async Task< IEnumerable<Katalog>> Get()
        {
            return await _repository.Get();
            //  throw new Exception("NOt Implimetn Exception");
        }


        // api/katalog (post) создать
        [HttpPost]
        public async Task< ActionResult<Katalog>> Post(Katalog item)
        {
            if (item == null)
            {
                return BadRequest();
            }
          // Console.WriteLine("Task< ActionResult<Katalog>> Post(Katalog item)----"+item.Name +"-"+item.Id+"-"+item.Model);
          if( await _repository.Add(item))
          {           
            return Ok(item);
          }
          else{
              return BadRequest("Ошибка субд,запись в бд не создана!");
          }
        }

        // PUT api/katalog/ (put) -изменить
        [HttpPut]
        public async Task< ActionResult<Katalog>> Put(Katalog item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            if (!await _repository.Update(item))
            {
                return NotFound(" item в субд не существует или ошибка связанная с обновлением в субд");
            }
 
            
            return Ok(item);
        }

        // DELETE api/katalog/5
        [HttpDelete("{id}")]
        public async Task< ActionResult<Katalog>> Delete(int id)
        {
           if (!await _repository.Delete(id))
            {
                return NotFound();
            }
            return Ok();
        }



        

    }





}