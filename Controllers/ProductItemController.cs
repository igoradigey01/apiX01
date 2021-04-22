using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebShopAPI.Model;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using ShopDbLibNew;
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
        private readonly UploadImageRepository _imageRepository;
        public ProductItemController(
            ProductItemRepository repository,
               UploadImageRepository imageRepository
            )
        {
            _repository = repository;
            _imageRepository = imageRepository;

        } //товарная позиция


        // воозвращает path-фото колекция (детарльное фото[] Товара)
      //---  [Route("api/[controller]/[action]")]--
        [HttpGet("{id}")]
        public async Task<string[]> GetImages(int idProduct)
        {
            // return "test-Get_TypeProductController";
            //  test_MySql();
            // throw new Exception("NOt Implimetn Exception");
            //  return await _repository.Get();
            Console.WriteLine("test GetImages ProductItemController"+" --IdProduct--"+idProduct.ToString());
            return await _repository.GetImags(idProduct);

        }

        // api/item-product (post) создать
        [HttpPost("{id}")]
        public async Task<ActionResult<bool>> AddImage(int idProduct)
        {

            Image itemProductImg = new Image { Id = 1, Name = "", ProductId = idProduct, Path = "" };
            IFormCollection form = await Request.ReadFormAsync();
            if (form == null)
            {
                return BadRequest("angular form data ==null");
            }
            if (form.Files.Count > 0)
            {

                var file = form.Files[0] as IFormFile;
                //  Console.WriteLine("molel controller post file name---"+file.Name);
                // var f=file as IFormFile;

                if (file != null)
                {
                    var path =
                    _imageRepository.GetImgPathNewName(
                         file.Name
                         );

                    _imageRepository.Save(path, file);
                    itemProductImg.Name = Path.GetFileName(path);
                    var flag = await _repository.AddImage(itemProductImg);
                    if (flag.Flag)
                    {
                        return Ok(flag.Flag);
                    }
                    else
                    {
                        return BadRequest(flag.Message);
                    }


                }
                else
                {
                    return BadRequest("angular-form--data невыбрано фото!");
                }
            }
            else
            {
                return BadRequest("angular-form--data невыбрано фото!");

            }
        }

        public async Task<ActionResult<bool>> UpdateImage(int idProcuct)
        {
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
                    _imageRepository.Save(path, file);
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