using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ShopDb;
using System;

namespace ShopAPI.Controllers
{
   
    [ApiController]
    [Authorize(Roles = Role.Admin + "," + Role.Furniture)]
    [Route("api/[controller]/[action]")]
    public class NomenclatureController : ControllerBase
    {
        private readonly NomenclatureRepository _repository;
        private readonly ImageRepository _imageRepository;

        public NomenclatureController(
            NomenclatureRepository repository,
             ImageRepository imageRepository

            )
        {
            _repository = repository;
            _imageRepository = imageRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<Nomenclature>> Get()
        {
            // int i = 0;
            return await _repository.Get();
        }

        [HttpGet("{idPostavchik}")]
        [AllowAnonymous]
        public async Task<IEnumerable<Nomenclature>> GetNomenclaturePs(int idPostavchik)
        {
            // int i = 0;
            return await _repository.GetNomenclatures(idPostavchik);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<Nomenclature> Item(int id)
        {
            return await _repository.Item(id);
            //  throw new Exception("NOt Implimetn Exception");
        }

        // POST api/<CategoriaController>
        // api/Material (post) создать
        [HttpPost]
        public async Task<ActionResult<Nomenclature>> Create()
        {
           // throw new NotImplementedException();

            Nomenclature item = new Nomenclature();
            IFormCollection form = await Request.ReadFormAsync();
            if (form == null)
            {
                return BadRequest("form data ==null");
            }

            item.Id = int.Parse(form["id"]);


            item.Name = form["name"];
            item.Name = item.Name.Trim();

            item.Hidden = bool.Parse(form["hidden"]);
          
            item.Price = int.Parse(form["price"]);
            item.Markup = int.Parse(form["markup"]);
            item.Description = form["description"];
            item.Position=int.Parse(form["position"]);
           // item.Guid = Guid.NewGuid().ToString();
            item.ArticleId= int.Parse(form["articleId"]);
            item.BrandId = int.Parse(form["brandId"]);
            item.KatalogId = int.Parse(form["katalogId"]);
            item.ColorId= int.Parse(form["colorId"]);
            item.PostavchikId = int.Parse("postavchikId");

            // throw  new Exception("not implict ");//14.03.22

            if (form.Files.Count > 0)
            {
                var file = form.Files[0] as IFormFile;
               if (file != null) return BadRequest("form.Files[0] == null"); ;
                var imgName = _imageRepository.RamdomName;

                _imageRepository.Save(imgName, file.OpenReadStream());

                item.Guid = imgName;
            }
            else
            {
                item.Guid = "not_found";

            }



            // Console.WriteLine("Task< ActionResult<Katalog>> Post(Katalog item)----"+item.Name +"-"+item.Id+"-"+item.Model);
            var flag = await _repository.Create(item);
            if (flag.Flag)
            {
                return Ok(flag.Item as Nomenclature);
            }
            else
            {
                return BadRequest(flag.Message);
            }
        }


        // public void Put(int id, [FromBody] string value)     

        // PUT api/material/3 (put) -изменить
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAll(int id)
        {
         //   throw new NotImplementedException();
            Nomenclature item = new Nomenclature();
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

            item.Hidden = bool.Parse(form["hidden"]);
            item.Price = int.Parse(form["price"]);
            item.Markup = int.Parse(form["markup"]);
            item.Description = form["description"];
            item.Position = int.Parse(form["position"]);
            item.Guid = form["guid"];
            item.ArticleId = int.Parse(form["articleId"]);
            item.BrandId = int.Parse(form["brandId"]);
            item.KatalogId = int.Parse(form["katalogId"]);
            item.ColorId = int.Parse(form["colorId"]);
            item.PostavchikId = int.Parse("postavchikId");


            
            var flagGuid = Guid.TryParse(item.Guid, out var i);

            if (!flagGuid)
            {
                item.Guid = _imageRepository.RamdomName;
            }
            if (form.Files.Count > 0)
            {
                var file = form.Files[0] as IFormFile;
                //   var imgName = _imageRepository.RamdomName;

                _imageRepository.Save(item.Guid, file.OpenReadStream());


            }
            else
            {
                item.Guid = "not_found";

            }
            // if(id!=item.Id) return BadRequest();
            var flag = await _repository.Update(item);
            if (flag.Flag)
            {
                // Katalog katalog = flag.Item as Katalog;
                //  Console.WriteLine(katalog.Name + "-----" + katalog.Id);
                return Ok();

            }


            return BadRequest(flag.Message);
        }

        [HttpPut("UpdateIgnoreImg/{id}")]
        public async Task<ActionResult<Nomenclature>> UpdateIgnoreImg(int id)
        {
           // throw new NotImplementedException();
            Nomenclature item = new Nomenclature();
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

         /*   string katalogName = form["katalogName"];
            //  добавить к имени название каталога
            var kn = katalogName.Trim();
            if (!item.Name.StartsWith(kn))
            {
                item.Name = kn + " " + item.Name;
            }*/

            item.KatalogId = int.Parse(form["katalogId"]);
            item.PostavchikId = int.Parse(form["postavchikId"]);
            item.ColorId = int.Parse(form["colorId"]);
            item.Price = int.Parse(form["price"]);
            item.Markup = int.Parse(form["markup"]);
            item.Description = form["description"];
            //  item.Image= form["imgName"]; !!!21.02.22
            item.Guid = form["guid"];
            var flagGuid = Guid.TryParse(item.Guid, out var i);
            if (flagGuid)
            {
                return BadRequest(" Guid img Незадан");
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

        [HttpPut("UpdateOnlyImg/{id}")]
        public async Task<ActionResult<Product>> UpdateOnlyImg(string id)
        {
          
            IFormCollection form = await Request.ReadFormAsync();


            if (form == null)
            {
                return BadRequest("form data ==null");
            }

            var nameImg = form["guid"];

            if (nameImg != id)
            {
                return BadRequest("Неверный Guid img ");
            }

            var flagGuid = Guid.TryParse(nameImg, out var i);

            if (!flagGuid)
            {
                return BadRequest(" Guid img Незадан");
            }


            if (form.Files.Count > 0)
            {
                var file = form.Files[0] as IFormFile;
                //   var imgName = _imageRepository.RamdomName;
                if (file != null) return BadRequest("form.Files[0] == null"); ;
                _imageRepository.Save(nameImg, file.OpenReadStream());

                return Ok();
            }



            return BadRequest("error -- form.Files[0] ");



        }

        // DELETE api/<CategoriaController>/5       
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
