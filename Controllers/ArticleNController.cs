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
    public class ArticleNController : ControllerBase
    {
        private readonly ArticleNRepository _repository;

        public ArticleNController(
            ArticleNRepository repository
            )
        {
            _repository = repository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<ArticleN>> Get()
        {
            // int i = 0;
            return await _repository.Get();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IEnumerable<ArticleN>> GetPostavchik(int id )
        {
            // int i = 0;
            return await _repository.GetPostavchik(id);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ArticleN> Item(int id)
        {
            return await _repository.Item(id);
            //  throw new Exception("NOt Implimetn Exception");
        }

        // POST api/<CategoriaController>
        // api/Material (post) создать
        [HttpPost]
        public async Task<ActionResult<ArticleN>> Create()
        {


            ArticleN item = new ArticleN();
            IFormCollection form = await Request.ReadFormAsync();
            if (form == null)
            {
                return BadRequest("form data ==null");
            }

            item.Id = int.Parse(form["id"]);


            item.Name = form["name"];
            item.Name = item.Name.Trim();

            item.Hidden = bool.Parse(form["hidden"]);
            
            item.PostavchikId = int.Parse(form["postavchikId"]);





            // Console.WriteLine("Task< ActionResult<Katalog>> Post(Katalog item)----"+item.Name +"-"+item.Id+"-"+item.Model);
            var flag = await _repository.Create(item);
            if (flag.Flag)
            {
                return Ok(flag.Item as ArticleN);
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

            ArticleN item = new ArticleN();
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
            item.PostavchikId = int.Parse(form["postavchikId"]);

            item.Hidden = bool.Parse(form["hidden"]);
          



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
