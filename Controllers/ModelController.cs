using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopDbLib.Models;
using WebShopAPI.Model; 
using Microsoft.AspNetCore.Authorization;



namespace WebShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class ModelController : ControllerBase
    {
        
        private readonly ModelRepository _repository;
        public ModelController(ModelRepository repository)
        {
            _repository = repository;

        }
        [HttpGet]        
        public async Task< IEnumerable<ShopDbLib.Models.Model>> Get()
        {
            return await _repository.Get();
            //  throw new Exception("NOt Implimetn Exception");
        }
        [HttpGet("{idKatalog}")]
        public async Task<IEnumerable<ShopDbLib.Models.Model>> Get(int idKatalog){
          return await _repository.Get(idKatalog);
            //  throw new Exception("NOt Implimetn Exception");
        }


        // api/katalog (post) создать
        [HttpPost]
        public async Task< ActionResult<ShopDbLib.Models.Model>> Post(ShopDbLib.Models.Model item)
        {
            if (item == null)
            {
                return BadRequest();
            }
           Console.WriteLine("Task< ActionResult<Model>> Post(Model item)----"+item.Name +"-"+item.Id+"-"+item.KatalogId);
          
         
          if( await _repository.Add(item))
          {           
            return Ok(item);
          }
          else{
              return BadRequest("Ошибка субд,запись в бд не создана!");
          }
        }

        // PUT api/katalog/ (put) -изменить
        [HttpPut("{id}")]
        public async Task< ActionResult<ShopDbLib.Models.Model>> Put(int id, ShopDbLib.Models.Model item)
        {

            if (item == null)
            {
                return BadRequest();
            }
            if(id!=item.Id) return BadRequest();
            
            if (!await _repository.Update(item))
            {
                return NotFound(" item в субд не существует или ошибка связанная с обновлением в субд");
            }
 
            
            return Ok(item);
        }

        // DELETE api/katalog/5
        [HttpDelete("{id}")]
        public async Task< ActionResult<ShopDbLib.Models.Model>> Delete(int id)
        {
           if (!await _repository.Delete(id))
            {
                return NotFound();
            }
            return Ok();
        }



    }
}