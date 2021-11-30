using System.ComponentModel.DataAnnotations;

namespace ShopAPI.Model
{
    public class LoginInputModelDto
    {
     
        public string ReturnUrl { get; set; }

        [Required]
        [Display(Name = "email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }
    }
}