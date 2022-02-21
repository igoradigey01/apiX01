using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopDb;
using ShopAPI.Model;
using Microsoft.AspNetCore.Authorization;
using System.IO;




namespace ShopAPI.Controllers
{

    [Route("api/[controller]")]
    [Authorize(Roles = Role.Admin + "," + Role.Manager)]
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
        [AllowAnonymous]
        public async Task<IEnumerable<Product>> Get(int idKatalog)
        {
            return await _repository.Get(idKatalog);
            //  throw new Exception("NOt Implimetn Exception");
        }

        // api/product (post) создать
        [HttpPost("Create")]
        public async Task<ActionResult<Product>> Post()
        {
          
            Product item = new Product();
     
            IFormCollection form = await Request.ReadFormAsync();

            if (form == null)
            {
                return BadRequest("angular form data ==null");
            }


            // ModelSerialize itemSerialize=form as ModelSerialize; не работает
            item.Name = form["name"];
            item.Name= item.Name.Trim();

            string katalogName = form["katalogName"];
            var kn=katalogName.Trim();
            if (!item.Name.StartsWith(kn))
            {
                item.Name = kn + " " + item.Name;
            }
            

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


            var file = form.Files[0] as IFormFile;
            if (file != null)
            {
                var imgName = _imageRepository.RamdomName;

                _imageRepository.Save(imgName, file.OpenReadStream());

                item.Image = imgName;
            }
            else
            {
                item.Image = "not_found";

            }

            // Console.WriteLine("Task< ActionResult<Model>> Post(Model item)----"+item.Name +"-"+item.Id+"-"+item.KatalogId);
            var flag = await _repository.Create(item);

            if (flag.Flag)
            {
                return Ok(item);
            }
            else
            {
                return BadRequest(flag.Message);
            }



        }

        // PUT api/katalog/5 (put) -изменить
        [HttpPut("UpdateAll/{id}")]       
        public async Task<ActionResult<Product>> UpdateAll(int id)
        {
            //  throw new Exception("NOt Implimetn Exception");
            Product item = new Product();
            IFormCollection form = await Request.ReadFormAsync();
            if (form == null)
            {
                return BadRequest("form data ==null");
            }
          
            item.Id = int.Parse(form["id"]);
            if (item.Id != id)
            {
                return BadRequest("Неверный Id");
            }
       
            item.Name = form["name"];
            item.Name = item.Name.Trim();

            string katalogName = form["katalogName"];
            var kn = katalogName.Trim();
            if (!item.Name.StartsWith(kn))
            {
                item.Name = kn + " " + item.Name;
            }

            item.KatalogId = int.Parse(form["katalogId"]);
            item.TypeProductId = int.Parse(form["typeProductId"]);
            item.Price = int.Parse(form["price"]);
            item.Markup = int.Parse(form["markup"]);
            item.Description = form["description"];



            IFormFile file = form.Files[0] as IFormFile;
            string? iname = form["imgName"];
          

            if (String.IsNullOrEmpty(iname)||String.Equals(iname,"null"))
            {
                item.Image= _imageRepository.RamdomName;
            }
            else
            {
                item.Image = iname;
            }
            if (file != null)
            {
               

                _imageRepository.Save(item.Image, file.OpenReadStream());

               
            }
           

           
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

        [HttpPut("UpdateIgnoreImg/{id}")]
        public async Task<ActionResult<Product>> UpdateIgnoreImg(int id)
        {
            //  throw new Exception("NOt Implimetn Exception");
            Product item = new Product();
            IFormCollection form = await Request.ReadFormAsync();
            if (form == null)
            {
                return BadRequest("form data ==null");
            }

            item.Id = int.Parse(form["id"]);
            if (item.Id != id)
            {
                return BadRequest("Неверный Id");
            }

            item.Name = form["name"];
            item.Name = item.Name.Trim();

            string katalogName = form["katalogName"];
            var kn = katalogName.Trim();
            if (!item.Name.StartsWith(kn))
            {
                item.Name = kn + " " + item.Name;
            }

            item.KatalogId = int.Parse(form["katalogId"]);
            item.TypeProductId = int.Parse(form["typeProductId"]);
            item.Price = int.Parse(form["price"]);
            item.Markup = int.Parse(form["markup"]);
            item.Description = form["description"];
          //  item.Image= form["imgName"]; !!!21.02.22

        
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

        [HttpPut("UpdateOnlyImg/{id}")]
        public async Task<ActionResult<Product>> UpdateOnlyImg(string id)
        {
            //  throw new Exception("NOt Implimetn Exception");
        

           

            IFormCollection form = await Request.ReadFormAsync();
            if (form == null)
            {
                return BadRequest("form data ==null");
            }

           

            var nameImg = form["imgName"];
            if (nameImg != id)
            {
                return BadRequest("Неверный imgName");
            }

            //  if(String.IsNullOrEmpty)
            if (String.IsNullOrEmpty(nameImg) || String.Equals(nameImg, "null"))
            {
                return BadRequest("NameImg == null /Имя фото незадано ");
            }
          



            var file = form.Files[0] as IFormFile;
           
            if (file != null || !String.IsNullOrEmpty(nameImg))
            {


                _imageRepository.Save(nameImg, file.OpenReadStream());

                return Ok(); 

            }

            return BadRequest("Фото с таким именем несуществует в репозитории или непередан blob obj");

           
        }

        // DELETE api/katalog/5
        [HttpDelete("Delete/{id}")]       
        public async Task<ActionResult> Delete(int id)
        {
            // throw new Exception("NOt Implimetn Exception");
       //     return BadRequest("test delete bad");
            Product item = await _repository.Item(id);
            var item_loads_Images = await _repository.ItemLoadChild(item);
            List<string> images = item_loads_Images.Images.Select(i => i.Name).ToList<string>();
            images.Add(item.Image);
            RepositoryResponseDto flag = await _repository.Delete(item_loads_Images);

            if (!flag.Flag)
            {
                return BadRequest(flag.Message);
            }
            foreach (var i in images)
            {
                _imageRepository.Delete(i);
            }

            return Ok();


        }




    }
}