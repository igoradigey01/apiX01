using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Model;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using ShopDb;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ShopAPI.Controllers
{
    //товарная позиция
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductItemController : ControllerBase //товарная позиция
    {
        private readonly ProductItemRepository _repository;
        private readonly ImageRepository _imageRepository;
        public ProductItemController(
            ProductItemRepository repository,
               ImageRepository imageRepository
            )
        {
            _repository = repository;
            _imageRepository = imageRepository;

        } //товарная позиция


        // воозвращает path-фото колекция (детарльное фото[] Товара)
        //---  [Route("api/[controller]/[action]")]--
        [HttpGet("{idProduct}")]
        public async Task<IEnumerable<ImageP>> GetImages(int idProcuct)
        {
            // return "test-Get_TypeProductController";
            //  test_MySql();
            // throw new Exception("NOt Implimetn Exception");
            //  return await _repository.Get();
            Console.WriteLine("test GetImages ProductItemController" + " --IdProduct--" + idProcuct.ToString());
            return await _repository.GetImags(idProcuct);

        }
        
        [HttpGet("{id}")]
        public async Task<Product> GetItemProducts(int id){
             return await _repository.GetItemProducts(id);
        }

        // api/item-product (post) создать
        [HttpPost]
        public async Task<ActionResult<ImageP>> CreateImage()
        {

            ImageP item = new ImageP { Id = 0, Name = "", ProductId = -1 };
            IFormCollection form = await Request.ReadFormAsync();
            if (form == null)
            {
                return BadRequest("angular form data ==null");
            }

            item.Id = int.Parse(form["id"]);
            item.ProductId = int.Parse(form["productId"]);

            if (item.Id == -1)
            {
                item.Id = 0;
            }
           
            item.Name = form["name"];

            //  Console.WriteLine("molel controller post file name---"+file.Name);
            // var f=file as IFormFile;
            if (String.IsNullOrEmpty(item.Name))
            {
                return BadRequest("angular form data item.name==null ");
            }

            var imgBase64String = form["imageBase64"];

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
            byte[] blobimg = _imageRepository.Base64ImgConvertor(imgBase64String);
            if (blobimg == null)
            {
                return BadRequest("angular form data item.imageBase64 img not png  format-- ");
            }

            _imageRepository.Save(imgName, blobimg); ;
            //   _imageRepository.SaveBase64Img(path,imgBase64String);

            // _imageRepository.Save(path, file);
            item.Name = imgName;  //----04.05.21  throw new Exception("Длина строки больше 6 символов");

            var flag = await _repository.CreateImage(item);
            if (flag.Flag)
            {
                return Ok(flag.Item);
            }
            else
            {
                return BadRequest(flag.Message);
            }



        }
       
         //    [HttpPut] -- не ипользуется толко добавляем или удаляем на клиенте
        public async Task<ActionResult<ImageP>> UpdateImage(int idImage)
        {
           throw new Exception("NOt Implimetn Exception");

            IFormCollection form = await Request.ReadFormAsync();
            if (form == null)
            {
                return BadRequest("angular form data ==null");
            }

            string imgBase64String = form["imageBase64"];

            if (String.IsNullOrEmpty(imgBase64String))
            {
                return BadRequest("angular form data item.imageBase64==null ");

            }

            //  var file = form.Files[0] as IFormFile;
            //  Console.WriteLine("molel controller post file name---"+file.Name);

            string name = form["name"];
            byte[] blobimg = _imageRepository.Base64ImgConvertor(imgBase64String);
            if (blobimg == null)
            {
                return BadRequest("angular form data item.imageBase64 img not png  format-- ");
            }

            _imageRepository.Save(name, blobimg); ;
            //   _imageRepository.SaveBase64Img(path,imgBase64String);

            // _imageRepository.Save(path, file);



            return Ok(true); ;

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
           // throw new Exception("NOt Implimetn Exception");
           ImageP item = await _repository.GetItemImage(id);
            if (item== null)
        {
            return BadRequest("Image not found in БД");
        }
            Console.WriteLine($"delete img-- name--{item.Name}--id--{item.Id}--OK()-- ");
            var flagValid =await  _repository.DeleteImage(item);
            _imageRepository.Delete(item.Name);
            if (!flagValid.Flag)
            {
               
                return BadRequest(flagValid.Message);
            }
            return Ok();
        }

        // возвращает позиции номенклатуры связанные с товарной позицией // ручки наравляющие (35 мм)
        [HttpGet]
        public async Task<IEnumerable<Nomenclature>> GetNomenclatures()
        {
            // return "test-Get_TypeProductController";
            //  test_MySql();
            throw new Exception("NOt Implimetn Exception");
            //  return await _repository.Get();

        }
    }
}