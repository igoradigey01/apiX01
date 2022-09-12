using System.ComponentModel.DataAnnotations;
namespace ShopAPI.Model
{
    public class ResetPasswordProfileDto
    {
        [Required(ErrorMessage = "OldPassword is required")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "NewPassword is required")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is required")]        
        public string Email { get; set; }
        
    }
}
