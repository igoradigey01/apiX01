using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebShopAPI.Model;
using ShopDbLib.Models;

namespace WebShopAPI.Model
{
    public class KatalogRepository
    {
        private readonly AppDbContext _db;

        public KatalogRepository(
            AppDbContext db
            )
        {
            _db = db;
        }

        public IQueryable<Katalog> GetKatalogs()
        {
            Console.WriteLine("Create -----------      GetProductTypes() ---------- Start->");
          //  throw new Exception("not implimetn exeption 14.11.20");
          return _db.Katalog;
        }
        
    }

}