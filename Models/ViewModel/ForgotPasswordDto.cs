using System.ComponentModel.DataAnnotations;
namespace ShopAPI.Model
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string ClientURI { get; set; }
    }
}
