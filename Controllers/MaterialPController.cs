using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ShopDb;

namespace ShopAPI.Controllers
{

    [ApiController]
    [Authorize(Roles = Role.Admin + "," + Role.Manager)]
    [Route("api/[controller]/[action]")]
    public class MaterialPController : ControllerBase
    {
        private readonly MaterialPRepository _repository;

        public MaterialPController(
             MaterialPRepository repository
        )
        {
            _repository = repository;

        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<MaterialP>> Get()
        {
            // return "test-Get_TypeProductController";
            //  test_MySql();
            // throw new Exception("NOt Implimetn Exception");
            return await _repository.Get();

        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<MaterialP> Item(int id)
        {
            return await _repository.Item(id);
            //  throw new Exception("NOt Implimetn Exception");
        }

        // api/Material (post) создать
        [HttpPost]
        public async Task<ActionResult<MaterialP>> Create()
        {


            MaterialP item = new MaterialP();
            IFormCollection form = await Request.ReadFormAsync();
            if (form == null)
            {
                return BadRequest("form data ==null");
            }

            item.Id = int.Parse(form["id"]);


            item.Name = form["name"];
            item.Name = item.Name.Trim();
            
            item.Hidden = bool.Parse(form["hidden"]);
            item.Description = form["description"];
            item.Description = item.Description.Trim();

            


            // Console.WriteLine("Task< ActionResult<Katalog>> Post(Katalog item)----"+item.Name +"-"+item.Id+"-"+item.Model);
            var flag = await _repository.Create(item);
            if (flag.Flag)
            {
                return Ok(flag.Item as MaterialP);
            }
            else
            {
                return BadRequest(flag.Message);
            }
        }

        // PUT api/material/3 (put) -изменить
        [HttpPut("{id}")]
        //[AllowAnonymous]
        public async Task<ActionResult> Update(int id)
        {

            MaterialP item = new MaterialP();
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
            item.Description= form["description"];
            item.Description=item.Description.Trim();
     
            


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

        // DELETE api/material/5
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