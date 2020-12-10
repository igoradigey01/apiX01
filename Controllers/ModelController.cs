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
         private readonly UploadImageRepository _imageRepository;
        
        public ModelController(
            ModelRepository repository,
            UploadImageRepository imageRepository
        )
        {
            _repository = repository;
            _imageRepository=imageRepository;           

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
        public async Task< ActionResult< ModelSerialize>> Post( ModelSerialize itemSerialize)
        {   ShopDbLib.Models.Model item= new ShopDbLib.Models.Model();
            
             // throw new Exception("NOt Implimetn Exception");
             item.Id=0;
             item.Name=itemSerialize.Name;
             item.KatalogId=itemSerialize.IdKatalog;
             item.Price=itemSerialize.Price;
             item.Markup=itemSerialize.Markup;
             item.Description=itemSerialize.Description;
             item.Image=
             _imageRepository.GetImgPathNewName(
                  itemSerialize.Photo.Name
                  );
            //  _imageRepository.Save(item.Image,itemSerialize.Photo);
            if (item == null)
            {
                return BadRequest();
            }
           Console.WriteLine("Task< ActionResult<Model>> Post(Model item)----"+item.Name +"-"+item.Id+"-"+item.KatalogId);
          
         
          if( await _repository.Add(item,_imageRepository.Save,itemSerialize.Photo))
          {           
            return Ok(item);
          }
          else{
              return BadRequest("Ошибка субд,запись в бд не создана!");
          }
        }

        // PUT api/katalog/ (put) -изменить
        [HttpPut("{id}")]
        public async Task< ActionResult<ModelSerialize>> Put(int id, ModelSerialize itemSerialize)
        {
           // throw new Exception("NOt Implimetn Exception");
           // ShopDbLib.Models.Model item

            if (itemSerialize == null)
            {
                return BadRequest();
            }
            if(id!=itemSerialize.Id) return BadRequest();
            
            if (!await _repository.Update(itemSerialize,_imageRepository.Update,itemSerialize.Photo))
            {
                return NotFound(" item в субд не существует или ошибка связанная с обновлением в субд");
            }

  
            
            return Ok(itemSerialize);
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