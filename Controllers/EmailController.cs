using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmailService;


namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailSender _emailSender;

        public EmailController(
           IEmailSender emailSender
            )
        {
           _emailSender = emailSender;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post()
        {
            var message = new Message(new string[] { "agape962@mail.ru" }, "Test email", "This is the content from our email.", null);
            _emailSender.SendEmail(message);

            return Ok();

        }
    }
}
