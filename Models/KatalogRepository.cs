using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DbClassLib.Models;

    
namespace AuthApi.Model
{
public class KatalogRepository
{
     private readonly MyShopContext _db;

     public  KatalogRepository( 
         MyShopContext  db
         )
    { 
        _db=db;
    }
     
    public IQueryable<Katalog> GetKatalogs(){
         Console.WriteLine("Create -----------      GetProductTypes() ---------- Start->");
        return _db.Katalog ;
    }
}

}