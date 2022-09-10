using System;
using System.ComponentModel.DataAnnotations;
//using Newtonsoft.Json;
using ShopDb;


namespace ShopAPI.Model
{
    // Аунтефикация Auth------------------------  

    public class UserProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //  public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }





}
