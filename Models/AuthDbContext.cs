using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
//using ShopDbLib.Models;

// partial class должны быть в одном и томже namespace !!!!
namespace WebShopAPI.Model
{

    public partial class AppDbContext : DbContext
    {
        IConfiguration _configuration;

        public DbSet<User> Users { get; set; }

       


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_configuration["ConnectStringLocal"]);      //Startup.GetConnetionStringDB());
        }
        //  при создании бд  создается admin 
        
         partial  void OnModelCreatingPartial(ModelBuilder modelBuilder);
       partial  void OnModelCreatingPartial(ModelBuilder modelBuilder){

          OnModelAuthCreating(modelBuilder);
        }

        private void OnModelAuthCreating(ModelBuilder modelBuilder)
        {

            var initObject = _configuration.GetSection("Users");
            var admin = initObject.GetSection("Admin");
            var user = initObject.GetSection("User");
            string adminEmail = admin["Email"];// ";
            string adminPassword = admin["Password"];
            string userEmail = user["Email"]; //  "user@mail.ru";
            string userPassword = user["Password"];// "";
            string userPhone = user["Phone"];// "+79181111111";
            string userName = "user";

            // добавляем роли

            User adminUser = new User
            {
                Id = 1,
                Email = adminEmail,
                Password = adminPassword,
                Role = Role.Admin,
                Name = admin["Name"]
            ,
                Address = "",
                Phone = admin["Phone"]
            };
            User user1 = new User
            {
                Id = 2,
                Email = userEmail,
                Password = userPassword,
                Name = userName,
                Role = Role.User,
                Address = "",
                Phone = userPhone
            };
            // modelBuilder.Entity<User>().Property(u=>u.Role).HasDefaultValue(Role.User);

            modelBuilder.Entity<User>().HasData(new User[] { adminUser, user1 });
            base.OnModelCreating(modelBuilder);


        }

       


    }

}
