using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopDb;
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

        private readonly ImageRepository _imageRepository;

        public ProductController(
            ProductRepository repository,
            ImageRepository imageRepository
        )
        {
            _repository = repository;
            _imageRepository = imageRepository;

        }


        [HttpGet("{idKatalog}")]
        public async Task<IEnumerable<Product>> Get(int idKatalog)
        {
            return await _repository.Get(idKatalog);
            //  throw new Exception("NOt Implimetn Exception");
        }

        // api/product (post) создать
        [HttpPost]
        public async Task<ActionResult<Product>> Post()
        {
            // throw new Exception("NOt Implimetn Exception");
            Product item = new Product();
            //  Console.WriteLine("api/katalog (post) создать --------");


            IFormCollection form = await Request.ReadFormAsync();
            if (form == null)
            {
                return BadRequest("angular form data ==null");
            }


            // ModelSerialize itemSerialize=form as ModelSerialize; не работает
            item.Name = form["name"];

            if (!_repository.NameUnique(item.Name))
            {
                return BadRequest("Такое название  уже существует!");
            }
            // Console.WriteLine("itemSerialize.n"+item.Name);
            item.Id = int.Parse(form["id"]);
            if (item.Id == -1)
            {
                item.Id = 0;
            }


            item.KatalogId = int.Parse(form["katalogId"]);
            item.TypeProductId = int.Parse(form["typeProductId"]);
            item.Price = int.Parse(form["price"]);
            item.Markup = int.Parse(form["markup"]);
            item.Description = form["description"];
            string imgBase64String = form["imageBase64"];

            if (String.IsNullOrEmpty(imgBase64String))
            {
                return BadRequest("angular form data item.imageBase64==null ");

            }

            //  var file = form.Files[0] as IFormFile;
            //  Console.WriteLine("molel controller post file name---"+file.Name);

            var typeFile = "temp.png";
            var imgName =
            _imageRepository.GetImgRamdomName(
                 typeFile
                 );
             byte[] blobimg=_imageRepository.Base64ImgConvertor(imgBase64String);
             if(blobimg==null){
                  return BadRequest("angular form data item.imageBase64 img not png  format-- ");
             }

            _imageRepository.Save(imgName, blobimg);;
       //   _imageRepository.SaveBase64Img(path,imgBase64String);

            // _imageRepository.Save(path, file);
            item.Image = imgName;




            // Console.WriteLine("Task< ActionResult<Model>> Post(Model item)----"+item.Name +"-"+item.Id+"-"+item.KatalogId);


            if (await _repository.Create(item))
            {
                return Ok(item);
            }
            else
            {
                return BadRequest("Ошибка субд,запись в бд не создана!");
            }



        }

        // PUT api/katalog/ (put) -изменить
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Put(int id)
        {
        //  throw new Exception("NOt Implimetn Exception");
            Product item = new Product();
            IFormCollection form = await Request.ReadFormAsync();
            if (form == null)
            {
                return BadRequest("form data ==null");
            }
            item.Name = form["name"];
            item.Id = int.Parse(form["id"]);
            if (item.Id != id)
            {
                return BadRequest("Неверный Id");
            }
            Console.WriteLine("---------------- item.Id" + item.Id.ToString());

            item.KatalogId = int.Parse(form["katalogId"]);
            item.TypeProductId = int.Parse(form["typeProductId"]);
            item.Price = int.Parse(form["price"]);
            item.Markup = int.Parse(form["markup"]);
            item.Description = form["description"]; string imgBase64String = form["imageBase64"];

            if (String.IsNullOrEmpty(imgBase64String))
            {
                return BadRequest("angular form data item.imageBase64==null ");

            }

            //  var file = form.Files[0] as IFormFile;
            //  Console.WriteLine("molel controller post file name---"+file.Name);
           var imgName=   item.Image=form["image"];

             if(imgName.Length!=40){
            var typeFile = "temp.png";
             imgName =_imageRepository.GetImgRamdomName( typeFile);
             }
                Console.WriteLine(" if(item.Image.Length!=40){----"+item.Image.Length.ToString()+"---"+item.Image);
           
              byte[] blobimg=null;
          //  
              if(imgBase64String.Length>0){
                blobimg=_imageRepository.Base64ImgConvertor(imgBase64String);
              }
              else return BadRequest("angular form data item.imageBase64 img Null-- ");
             if(blobimg==null){
                  return BadRequest("angular form data item.imageBase64 img not png  format-- ");
             }

            _imageRepository.Update(imgName, blobimg);;
       //   _imageRepository.SaveBase64Img(path,imgBase64String);

            // _imageRepository.Save(path, file);
            item.Image = imgName;
            //--------------13.04.21----------------------------

            //  _imageRepository.Update(item.Image,file);---14.12.20
            var flag = await _repository.Update(item);
            if (flag.Flag)
            {
                return Ok(); ;
            }
            else
            {
                return BadRequest(flag.Message);
            }
        }

        // DELETE api/katalog/5
        [HttpDelete("{id}")]
       // [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            // throw new Exception("NOt Implimetn Exception");
            Product item=await _repository.Item(id);
            var item_loads_Images=await _repository.ItemLoadChild(item);
            List<string> images=item_loads_Images.Images.Select(i=>i.Name).ToList<string>();
            images.Add(item.Image);
            FlagValid flag=await _repository.Delete(item_loads_Images);
            if (!flag.Flag)
            {
                return BadRequest(flag.Message);
            }
            foreach(var i in images){
                _imageRepository.Delete(i);
            }

                return Ok();

       
        }




    }
}