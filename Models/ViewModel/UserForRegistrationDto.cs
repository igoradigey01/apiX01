using System;
using System.ComponentModel.DataAnnotations;

namespace ShopAPI.Model
{
    public class UserForRegistrationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Незадан Email")]
        [EmailAddress]
        public string Email { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "Незадан Телефон")]
        public string Phone { get; set; }



        [Required(ErrorMessage = "Незадан Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }

        public string ClientURI { get; set; }
    }
}
