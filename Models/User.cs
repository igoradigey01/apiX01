using System;
using Microsoft.AspNetCore.Identity;
namespace ShopAPI.Model{

    public class User: IdentityUser{
        public string Name{get;set;}
        public string Address{get;set;}
        public string Phone{get;set;}
        public string Password{get;set;}

    }

}