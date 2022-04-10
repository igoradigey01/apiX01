using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ShopDb;

namespace ShopAPI.Controllers
{

    [ApiController]
    [Authorize(Roles = Role.Admin + "," + Role.Furniture)]
    [Route("api/[controller]/[action]")]
    public class KatalogNController : ControllerBase
    {
        private readonly KatalogNRepository _repository;

        public KatalogNController(
            KatalogNRepository repository
            )
        {
            _repository = repository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<KatalogN>> Get()
        {
            // int i = 0;
            return await _repository.Get();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IEnumerable<KatalogN>> GetPostavchik(int id)
        {
            // int i = 0;
            return await _repository.GetPostavchik(id);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<KatalogN> Item(int id)
        {
            return await _repository.Item(id);
            //  throw new Exception("NOt Implimetn Exception");
        }


        [HttpGet("{idCategoriaN}")]
        [AllowAnonymous]
        public async Task<IEnumerable<KatalogN>> KatalogNs(int idCategoriaN)
        {
            return await _repository.KatalogNs(idCategoriaN);
            // throw new Exception("NOt Implimetn Exception");
        }

        // POST api/<CategoriaController>
        // api/Material (post) создать
        [HttpPost]
        public async Task<ActionResult<KatalogN>> Create()
        {


            KatalogN item = new KatalogN();
            IFormCollection form = await Request.ReadFormAsync();
            if (form == null)
            {
                return BadRequest("form data ==null");
            }

            item.Id = int.Parse(form["id"]);
            item.PostavchikId = int.Parse("postavchikId");

            item.Name = form["name"];
            item.Name = item.Name.Trim();

            item.Hidden = bool.Parse(form["hidden"]);
            item.CategoriaId = int.Parse(form["categoriaId"]);
            item.DecriptSEO = form["decriptSEO"];





            // Console.WriteLine("Task< ActionResult<Katalog>> Post(Katalog item)----"+item.Name +"-"+item.Id+"-"+item.Model);
            var flag = await _repository.Create(item);
            if (flag.Flag)
            {
                return Ok(flag.Item as KatalogN);
            }
            else
            {
                return BadRequest(flag.Message);
            }
        }


        // public void Put(int id, [FromBody] string value)     

        // PUT api/material/3 (put) -изменить
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id)
        {

            KatalogN item = new KatalogN();
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
            item.PostavchikId = int.Parse("postavchikId");
            item.Name = form["name"];
            item.Name = item.Name.Trim();

            item.Hidden = bool.Parse(form["hidden"]);
            item.CategoriaId = int.Parse(form["categoriaId"]);
            item.DecriptSEO = form["decriptSEO"];




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
