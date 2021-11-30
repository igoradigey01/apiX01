using System;
using Microsoft.AspNetCore.Identity;

namespace ShopAPI.Models
{

    public class User: IdentityUser{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address{get;set;}
        public string Phone{get;set;} //delete               
        public string RefreshToken{get;set;}

        public DateTime RefreshTokenExpiryTime{get;set;}
    }

}