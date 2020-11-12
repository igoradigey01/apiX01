using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AuthApi.Model
{
    public class AppDbContext:DbContext
    {
             IConfiguration _configuration;
        
            public DbSet<User> Users { get; set; }
            

           

            

        public AppDbContext(DbContextOptions<AppDbContext> options,IConfiguration configuration) {
          _configuration=configuration;

           
           Database.SetCommandTimeout(300);
          Database.EnsureDeleted();  //24.10.20
            Database.EnsureCreated();
            }

            
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
              optionsBuilder.UseMySql(_configuration["ConnectStringLocal"]);      //Startup.GetConnetionStringDB());
            }
                 //  при создании бд  создается admin 
            protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
          
            OnModelAuthCreating(modelBuilder);
           // OnModelProductTypeCreating(modelBuilder);
            
        }

        private void OnModelAuthCreating(ModelBuilder modelBuilder){

          var initObject=_configuration.GetSection("Users");
          var admin=initObject.GetSection("Admin");
          var user=initObject.GetSection("User");
          string adminEmail =  admin["Email"];// ";
            string adminPassword =  admin["Password"]; 
            string userEmail=user["Email"]; //  "user@mail.ru";
            string userPassword=user["Password"];// "";
            string userPhone= user["Phone"];// "+79181111111";
            string userName="user";
 
            // добавляем роли
            
            User adminUser = new User {Id=1,  Email = adminEmail, Password = adminPassword,
             Role = Role.Admin,Name=admin["Name"]
            ,Address="" ,Phone=admin["Phone"]};
            User user1= new User{Id=2, Email=userEmail,Password=userPassword,
            Name=userName,
            Role=Role.User,Address="",Phone=userPhone};
          // modelBuilder.Entity<User>().Property(u=>u.Role).HasDefaultValue(Role.User);
          
            modelBuilder.Entity<User>().HasData( new User[] { adminUser,user1});
            base.OnModelCreating(modelBuilder);


        }

 
}

}
