using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebShopAPI.Model;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using ShopDb;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace WebShopAPI.Controllers
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
        [HttpGet("{id}")]
        public async Task<IEnumerable<Image>> GetImages(int id)
        {
            // return "test-Get_TypeProductController";
            //  test_MySql();
            // throw new Exception("NOt Implimetn Exception");
            //  return await _repository.Get();
            Console.WriteLine("test GetImages ProductItemController" + " --IdProduct--" + id.ToString());
            return await _repository.GetImags(id);

        }

        // api/item-product (post) создать
        [HttpPost]
        public async Task<ActionResult<bool>> AddImage(){
            
            Image item = new Image { Id = 0, Name = "", ProductId = -1 };
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

            var path =
            _imageRepository.GetImgPathNewName(
                 item.Name
                 );
            byte[] imgBytes = Convert.FromBase64String(imgBase64String);


            _imageRepository.Save(path, imgBytes);  //----04.05.21  throw new Exception("Длина строки больше 6 символов");
            item.Name = Path.GetFileName(path);
            var flag = await _repository.AddImage(item);
            if (flag.Flag)
            {
                return Ok(flag.Flag);
            }
            else
            {
                return BadRequest(flag.Message);
            }



        }

    

    public async Task<ActionResult<bool>> UpdateImage(int idProcuct)
    { 
         throw new Exception("NOt Implimetn Exception");
        IFormCollection form = await Request.ReadFormAsync();
        if (form == null)
        {
            return BadRequest("angular form data ==null");
        }
        if (form.Files.Count > 0)
        {

            var file = form.Files[0] as IFormFile;
            if (file != null)
            {
                var path =
                _imageRepository.GetImgPathNewName(
                     file.Name
                     );
           //-----------------     _imageRepository.Save(path, file);
                return Ok(true);
            }
            return BadRequest("Фото не выбрано (front) !!");
        }
        return BadRequest("Фото не обновлено IO File server error!");

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