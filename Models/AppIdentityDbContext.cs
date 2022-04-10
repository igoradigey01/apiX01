using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;

namespace ShopAPI.Model
{
    public class AppIdentityDbContext : IdentityDbContext<User>
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
            const string Furniture_id = "Lc0f5ba0a-75d5-4e49-b500-c734291f9f9c";
            const string TKAN_ID = "Lc124d441-7c18-4f02-8e0c-705f24a32866";

            const string ROLE_ID = ADMIN_ID;

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = ROLE_ID,
                    Name = Role.Admin,            //"admin",
                    NormalizedName = Role.Admin
                },
                new IdentityRole
                {
                    Id = USER_ID,
                    Name = Role.Shopper,          //"shopper",
                    NormalizedName = Role.Shopper //покупатель

                },
                 new IdentityRole
                 {
                     Id = MANAGER_Id,
                     Name = Role.Manager,           //"manager",
                     NormalizedName = Role.Manager

                 },
                 new IdentityRole
                 {
                     Id = Furniture_id,
                     Name = Role.Furniture,
                     NormalizedName = Role.Furniture

                 },
                 new IdentityRole
                 {
                     Id = TKAN_ID,
                     Name = Role.TkanO,
                     NormalizedName = Role.TkanO

                 }

            );

            var passHash = new PasswordHasher<User>();

            builder.Entity<User>().HasData(new User[] {

                new User
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
                },
                      new User
            {
                Id = Furniture_id,
                UserName = "furniture",
                NormalizedUserName = "furniture",
                Email = "furniture@x-01.ru",
                NormalizedEmail = "furniture@x-01.ru",
                EmailConfirmed = true,
                PasswordHash = passHash.HashPassword(null, "i-1985"),
                PhoneNumber = Environment.GetEnvironmentVariable("AdminPhone"),
                SecurityStamp = string.Empty
            }
                }
            );


            builder.Entity<IdentityUserRole<string>>().HasData(

                new IdentityUserRole<string>[] {

                new IdentityUserRole<string>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            },
                 new IdentityUserRole<string>
                {
                    RoleId = Furniture_id,
                    UserId = Furniture_id
                }
                });



        }


    }
}
