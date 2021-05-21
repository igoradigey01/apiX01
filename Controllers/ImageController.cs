using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebShopAPI.Model;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Text;
namespace WebShopAPI.Controllers

{

    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        //-----------------------------
        string[] _l = new[] { "один", "два", "Три-test" };

        readonly ImageRepository _imageRepository;


        public ImageController( ImageRepository imageRepository )
        {
              _imageRepository = imageRepository;

        }



        [HttpGet("{name}")]

        public async Task<IActionResult> Get(string name)
        {
            var contentType = "image/png";
            //var stream = new MemoryStream(Encoding.ASCII.GetBytes("Hello World"));
            byte[] stream = _imageRepository.GetImage(name);
            return new FileContentResult(stream, contentType);
        }


    }

}