using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AuthApi.Model;
using Microsoft.AspNetCore.Authorization;
namespace AuthApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        //------------------------------------------
        User user1=new User {  Address="adress-1",Email="rr44@mail.ru",
        Name="User-1",Password="123",Phone="789456123",Role=Role.Admin};
        User user2=new User { Address="adress-2",Email="rr44@mail.ru",
        Name="User-2",Password="123",Phone="789456123",Role=Role.Admin};
        User user3=new User { Address="adress-3",Email="rr44@mail.ru",
        Name="User-3",Password="123",Phone="789456123",Role=Role.Admin};
        User user4=new User { Address="adress-4",Email="rr44@mail.ru",
        Name="User-4",Password="123",Phone="789456123",Role=Role.Admin};


 
        //-----------------------------
        string[] _l=new[]{"один","два","Три-test"};

        private readonly AuthRepository _repository;
          /// <summary>
          /// 
          /// </summary>
          /// <param name="repository"></param>
        public TestController(AuthRepository repository){
          _repository=repository;

        }
        
        
         [HttpGet]
        /// <summary>
        /// работа приложения на хосте; mysql connect - TestController(AuthRepository repository);
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Get()
            {
                
         //  test_MySql();
        return _l.ToArray();
            
        }

        private void test_MySql(){
        
            

            
                
               // RawSqlString str=
              // RawSqlString str=new RawSqlString("SELECT LAST_INSERT_ID();");
             
              
               
           
             

            
             //user1.Id=(id+1).ToString();
            // user2.Id=(id+2).ToString();
            // user3.Id=(id+3).ToString();
               //  _repository.CreateUser(user1);
              //   _repository.CreateUser(user2);16.09.20
                 //  _repository.CreateUser(user3);16.09.20
             //   var user=   _repository.GetUserId(user1.Id);
              //  user.Name="test chengesUser-1";
               // _repository.SeveUser(user);
              //  db.Users.Add(user2);
              //  db.Users.Add(user3);
             //   db.SaveChangesAsync();
                Console.WriteLine("Объекты успешно сохранены");
 
                var users = _repository.GetUsers();
                Console.WriteLine("Список объектов:");
                foreach (User u in users)
                {
                    Console.WriteLine($"{u.Id}.{u.Name} - {u.Address}");
                }
        
            
           



            
        }


    }
    
}