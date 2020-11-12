using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace AuthApi.Model
{
    // Аунтефикация Auth------------------------  
public interface IUser{
  public string Name{get;set;}
  public string  Password{get;set;}
  public string  Email{get;set;}
  public string  Address {get;set;}

  public string Phone{get;set;}

}

public class User:IUser{


public int Id{get;set;}

[Required(ErrorMessage = "Укажите имя пользователя")]
[MaxLength(50)]
 public  string Name{get;set;}
  [Required(ErrorMessage = "Задайте пароль")]
  [MaxLength(30)]
 public string Password{get;set;}
 
 [MaxLength(30)]
 public string Email{get;set;}
  [Required(ErrorMessage = "Укажите  телефон ")]
  [MaxLength(20)]
 public string Phone{get;set;}
 public string Address{get;set;}

 [MaxLength(10)]
   [Required]
 public Role Role{get;set;}
   }

public class UserSerialize:IUser{
  public string Name{get;set;}
  public string Password{get;set;}
  public string Email {get;set;}
  public string Phone{get;set;}
  public string Address{get;set;}
}


  
   public enum Role{
       User,Admin
   }
   public class Login{
       [Required]
       [EmailAddress]
       public string Email{get;set;}
       [Required]
       public string Password{get;set;}
   }

}
