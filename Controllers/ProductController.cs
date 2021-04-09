using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopDbLibNew;
using WebShopAPI.Model;
using Microsoft.AspNetCore.Authorization;
using System.IO;




namespace WebShopAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly ProductRepository _repository;
        private readonly UploadImageRepository _imageRepository;

        public ProductController(
            ProductRepository repository,
            UploadImageRepository imageRepository
        )
        {
            _repository = repository;
            _imageRepository = imageRepository;

        }


        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _repository.Get();
            //  throw new Exception("NOt Implimetn Exception");
        }
        [HttpGet("{idKatalog}")]
        public async Task<IEnumerable<Product>> Get(int idKatalog)
        {
            return await _repository.Get(idKatalog);
            //  throw new Exception("NOt Implimetn Exception");
        }


        // api/katalog (post) создать
        [HttpPost]
        public async Task<ActionResult<Product>> Post()
        {
           Product item = new Product();
          //  Console.WriteLine("api/katalog (post) создать --------");

           
                IFormCollection form  = await Request.ReadFormAsync();
                 if (form == null)
               {
                   return BadRequest("form data ==null");
               }
               
               
              // ModelSerialize itemSerialize=form as ModelSerialize; не работает
                   item.Name=form["name"];

                   if(!_repository.NameUnique(item.Name)){
                       return BadRequest("Такое название  уже существует!");
                   }
                 // Console.WriteLine("itemSerialize.n"+item.Name);
                  item.Id=int.Parse(form["id"]);
                  if(item.Id==-1){
                      item.Id=0;
                  }
               
                
                item.KatalogId=int.Parse( form["idKatalog"]);
                item.TypeProductId=int.Parse(form[""]);
                item.Price=int.Parse( form["price"]);
                item.Markup=int.Parse(form["markup"]);
                item.Description=form["description"];


                
                 var file=  form.Files[0] as IFormFile ;
               //  Console.WriteLine("molel controller post file name---"+file.Name);
                // var f=file as IFormFile;
                           
                 if(file!=null){
                var path=
                _imageRepository.GetImgPathNewName(
                     file.Name
                     );

                     

                 _imageRepository.Save(path,file);
                 item.Image=Path.GetFileName(path);

                 
                 }
             // Console.WriteLine("Task< ActionResult<Model>> Post(Model item)----"+item.Name +"-"+item.Id+"-"+item.KatalogId);


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
        public async Task<ActionResult<ModelSerialize>> Put(int id, ModelSerialize itemSerialize)
        {
            // throw new Exception("NOt Implimetn Exception");
            // ShopDbLib.Models.Model item

            if (itemSerialize == null)
            {
                return BadRequest();
            }
            if (id != itemSerialize.Id) return BadRequest();

          //  _imageRepository.Update(item.Image,file);---14.12.20

            if (!await _repository.Update(itemSerialize, itemSerialize.Photo))
            {
                return NotFound(" item в субд не существует или ошибка связанная с обновлением в субд");
            }



            return Ok(itemSerialize);
        }

        // DELETE api/katalog/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> Delete(int id)
        {
            if (!await _repository.Delete(id))
            {
                return NotFound();
            }
            return Ok();
        }




    }
}