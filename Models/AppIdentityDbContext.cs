using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;

namespace ShopAPI.Model
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
           : base(options)
        {

            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            const string ADMIN_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
            // any guid, but nothing is against to use the same one
            const string USER_ID = "2301D884-221A-4E7D-B509-0113DCC043E1";
            const string MANAGER_Id = "7D9B7113-A8F8-4035-99A7-A20DD400F6A3";
            const string ROLE_ID = ADMIN_ID;

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = ROLE_ID,
                    Name = "admin",
                    NormalizedName = "admin"
                },
                new IdentityRole
                {
                    Id = USER_ID,
                    Name = "shopper",
                    NormalizedName = "shopper" //покупатель

                },
                 new IdentityRole
                 {
                     Id = MANAGER_Id,
                     Name = "manager",
                     NormalizedName = "manager"

                 }

            );

            var passHash = new PasswordHasher<AppUser>();

            builder.Entity<AppUser>().HasData(

                new AppUser
                {
                    Id = ADMIN_ID,
                    UserName = "admin",
                    NormalizedUserName = "admin",
                    Email = Environment.GetEnvironmentVariable("AdminEmail"),
                    NormalizedEmail = Environment.GetEnvironmentVariable("AdminEmail"),
                    EmailConfirmed = true,
                    PasswordHash = passHash.HashPassword(null, Environment.GetEnvironmentVariable("AdminPass")),
                    PhoneNumber = Environment.GetEnvironmentVariable("AdminPhone"),
                    SecurityStamp = string.Empty
                }
            );

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });



        }


    }
}
