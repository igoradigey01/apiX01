using System;
using Microsoft.AspNetCore.Identity;

namespace ShopAPI.Model{

    public class AppUser: IdentityUser{
        public string Name{get;set;}
        public string Address{get;set;}
        public string Phone{get;set;} //delete
        public string Password{get;set;} // delete       
        public string RefreshToken{get;set;}

        public DateTime RefreshTokenExpiryTime{get;set;}
    }

}