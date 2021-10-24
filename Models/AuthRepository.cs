using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShopDb;

namespace ShopAPI.Model
{
public class AuthRepository
{
    //класс-репозиторий напрямую обращается к контексту базы данных
    private readonly MyShopContext context;


    public  AuthRepository(MyShopContext context)
    {
        this.context = context;
    }

    //выбрать все записи из таблицы 
    public IQueryable<AppUser> GetUsers()
    {
        throw new Exception("Exception --GetUser ---Not  implement  exception");
       // return context.Users.OrderBy(x => x.Name);
    }

    //найти определенную запись по id
    public AppUser GetUserId(int id)
    {
        throw new Exception("Exception --GetUserId(int id) ---Not  implement  exception");
        //return context.Users.Single(x => x.Id == id);
    }
   
    // Есть ли user 
    public AppUser ValidateUser(string email,string password){
        if((email!=null)&&(password!=null)){
          //  return  context.Users.SingleOrDefault(u=> u.Password==password && u.Email==email);
        }

        return null;
    }
   
     //--
    public bool ValidateEmailUser(string email){

        if(email==null){
            return false;
        }
        throw new Exception("Exception --GetUserId(int id) ---Not  implement  exception");
    //   User user=  context.Users.SingleOrDefault(u=>u.Email==email);
    //   if(user!=null){
    //       return false;
    //   }
      return true;
    }
           
    // если Phone существует -False (not Validate)
    public bool ValidatePhoneUser(string phone){
     if(phone==null){
         return false;
     }
     throw new Exception("Exception --GetUserId(int id) ---Not  implement  exception");
    //   User user =context.Users.SingleOrDefault(u=>u.Phone==phone);
    //   if(user!=null){
    //       return false;
    //   }
        return true;
    }

   
    // обновить существующую запись в бд
    public void SaveUser(AppUser user){
        context.Entry(user).State = EntityState.Modified;
        context.SaveChanges();
    }


    //сохранить новую запись в БД
    public void CreateUser(AppUser user)
    {
throw new Exception("Exception --GetUserId(int id) ---Not  implement  exception");
        // context.Users.Add(user);
        // context.SaveChanges();
       
          
        /*
        if (entity.Id == -1) //default
        
           
            context.Entry(entity).State = EntityState.Added;
        else
            context.Entry(entity).State = EntityState.Modified;
        context.SaveChanges();

        return entity.Id;
        */
        
    }
    
    //удалить существующую запись
    public void DeleteUser(AppUser entity)
    {
        throw new Exception("Exception --GetUserId(int id) ---Not  implement  exception");
        // context.Users.Remove(entity);
        // context.SaveChanges();
    }
}
}