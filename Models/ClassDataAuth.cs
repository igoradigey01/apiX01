using System;
using System.ComponentModel.DataAnnotations;
//using Newtonsoft.Json;
using ShopDb;


namespace WebShopAPI.Model
{
    // Аунтефикация Auth------------------------  

public class UserSerialize{
  public string Name{get;set;}
  public string Password{get;set;}
  public string Email {get;set;}
  public string Phone{get;set;}
  public string Address{get;set;}
}


  
     public class Login{
       [Required]
       [EmailAddress]
       public string Email{get;set;}
       [Required]
       public string Password{get;set;}
   }

}
