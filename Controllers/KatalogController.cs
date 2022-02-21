using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopDb;
using ShopAPI.Model;

using Microsoft.AspNetCore.Authorization;

namespace ShopAPI.Controllers
{
    [ApiController]
    [Authorize(Roles = Role.Admin+","+ Role.Manager)]
   // [Authorize(Roles = Role.Manager)]
    [Route("api/[controller]/[action]")]
    public class KatalogController : ControllerBase
    {
        private readonly KatalogRepository _repository;
        public KatalogController(KatalogRepository repository)
        {
            _repository = repository;

        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<Katalog>> Get()
        {
            return await _repository.Get();
            //  throw new Exception("NOt Implimetn Exception");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<Product>> Products(int id)
        {
            return await _repository.Get(id);
            //  throw new Exception("NOt Implimetn Exception");
        }


        // api/katalog (post) создать
        [HttpPost]       
        public async Task<ActionResult<Katalog>> Post()
        {
           

            Katalog item = new Katalog();
            IFormCollection form = await Request.ReadFormAsync();
            if (form == null)
            {
                return BadRequest("form data ==null");
            }

            item.Id = int.Parse(form["id"]);
           

            item.Name = form["name"];
            item.Name = item.Name.Trim();
            item.Flag_href = bool.Parse(form["flag_href"]);
            item.Flag_link = bool.Parse(form["flag_link"]);
            item.Hidden = bool.Parse(form["hidden"]);
            item.Link = form["link"];

            // Console.WriteLine("Task< ActionResult<Katalog>> Post(Katalog item)----"+item.Name +"-"+item.Id+"-"+item.Model);
            var flag = await _repository.Add(item);
            if (flag.Flag)
            {
                return Ok(item);
            }
            else
            {
                return BadRequest(flag.Message);
            }
        }

        // PUT api/katalog/ (put) -изменить
        [HttpPut("{id}")] 
        //[AllowAnonymous]
        public async Task<ActionResult> Update(int id)
        {

            Katalog item = new Katalog();
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
            item.Flag_href =bool.Parse( form["flag_href"]);
            item.Flag_link = bool.Parse(form["flag_link"]);
            item.Hidden = bool.Parse(form["hidden"]);
            item.Link = form["link"];


            // if(id!=item.Id) return BadRequest();
            var flag = await _repository.Update(item);
            if (flag.Flag)
            {
                Katalog katalog = flag.Item as Katalog;
                Console.WriteLine(katalog.Name + "-----" + katalog.Id);
                return Ok();

            }


            return BadRequest(flag.Message);
        }

        // DELETE api/katalog/5
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