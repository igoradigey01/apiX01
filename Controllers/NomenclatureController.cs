using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ShopDb;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShopAPI.Controllers
{
    public class PriceN
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("price")]
        public int Price { get; set; }
    }

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

        [HttpGet("{idPostavchik}")]
        [AllowAnonymous]  /**pastavchik get Nomenclature */
        public async Task<IEnumerable<Nomenclature>> NomenclaturePs(int idPostavchik)
        {
            // int i = 0;
            return await _repository.GetNomenclatures(idPostavchik);
        }

        [HttpGet("{idKatalog}")]
        [AllowAnonymous]     /**Postavchik + Katalog get Nomenclature*/
        public async Task<IEnumerable<Nomenclature>> NomenclaturePKs(int idKatalog, [FromQuery] int postavchikId)
        {
            // int i = 0;
            return await _repository.GetNomenclatures(idKatalog, postavchikId);
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
            if (item.Id == -1)
            {
                item.Id = 0;
            }


            item.Name = form["name"];
            item.Name = item.Name.Trim();

            item.Hidden = bool.Parse(form["hidden"]);
            item.InStock = bool.Parse(form["inStock"]);
            item.Sale = bool.Parse(form["sale"]);

            item.Price = int.Parse(form["price"]);
            item.Markup = int.Parse(form["markup"]);
            item.Description = form["description"];
            item.Position = int.Parse(form["position"]);
            // item.Guid = Guid.NewGuid().ToString();
            item.ArticleId = int.Parse(form["articleId"]);
            item.BrandId = int.Parse(form["brandId"]);
            item.KatalogId = int.Parse(form["katalogId"]);
            item.ColorId = int.Parse(form["colorId"]);
            item.PostavchikId = int.Parse(form["postavchikId"]);

            // throw  new Exception("not implict ");//14.03.22

            if (form.Files.Count > 0)
            {
                var file = form.Files[0] as IFormFile;
                if (file == null) return BadRequest("form.Files[0] == null"); ;
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
            item.InStock = bool.Parse(form["inStock"]);
            item.Sale = bool.Parse(form["sale"]);
            item.Price = int.Parse(form["price"]);
            item.Markup = int.Parse(form["markup"]);
            item.Description = form["description"];
            item.Position = int.Parse(form["position"]);
            item.Guid = form["guid"];
            item.ArticleId = int.Parse(form["articleId"]);
            item.BrandId = int.Parse(form["brandId"]);
            item.KatalogId = int.Parse(form["katalogId"]);
            item.ColorId = int.Parse(form["colorId"]);
            item.PostavchikId = int.Parse(form["postavchikId"]);



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
                item.Guid = "00000000-0000-0000-0000-000000000000";

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

        [HttpPut]
        
        public async Task<ActionResult> UpdateDataPrice()
        {
            //  Request.Body.
            //var items = await Request.ReadFromJsonAsync<PriceN[]>();
            //   var person = await request.ReadFromJsonAsync<Person>();

            string body = "";
            using (StreamReader stream = new StreamReader(Request.Body))
            {
                 body = await stream.ReadToEndAsync();

               
            }
               

                var items = JsonSerializer.Deserialize<PriceN[]>(body);
            if (items == null)
            {
                return BadRequest("form data ==null");
            }
            if (items.Length > 0) {
                var flag = await  _repository.UpdateDataPrice(items);
                if(flag.Flag) return Ok(items);
                else return BadRequest(flag.Message);
            }
            
            return BadRequest();

        }

        [HttpPut("{id}")]
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
            item.Name = item.Name;



            item.ArticleId = int.Parse(form["articleId"]);
            item.BrandId = int.Parse(form["brandId"]);
            item.KatalogId = int.Parse(form["katalogId"]);
            item.ColorId = int.Parse(form["colorId"]);
            item.PostavchikId = int.Parse(form["postavchikId"]);
            item.ColorId = int.Parse(form["colorId"]);
            item.Hidden = bool.Parse(form["hidden"]);
            item.InStock = bool.Parse(form["inStock"]);
            item.Sale = bool.Parse(form["sale"]);
            item.Price = int.Parse(form["price"]);
            item.Markup = int.Parse(form["markup"]);
            item.Description = form["description"];
            //  item.Image= form["imgName"]; !!!21.02.22
            item.Guid = form["guid"];
            var flagGuid = Guid.TryParse(item.Guid, out var i);
            if (!flagGuid)
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

        [HttpPut("{id}")]
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
                if (file != null)
                {
                    _imageRepository.Save(nameImg, file.OpenReadStream());

                    return Ok();
                }
            }



            return BadRequest("form.Files[0] == null"); 



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
