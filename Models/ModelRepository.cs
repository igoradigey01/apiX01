using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using  Microsoft.EntityFrameworkCore;
using ShopDbLib.Models;
namespace WebShopAPI.Model
{
    public class ModelRepository
    {
        private readonly AppDbContext _db;

        public ModelRepository( AppDbContext db )
        {
            _db = db;
        }

          public async Task< IEnumerable<ShopDbLib.Models.Model>> Get()
        {
             
          //  throw new Exception("not implimetn exeption 14.11.20");
          return  await _db.Model.ToListAsync();
          
        }

          public async Task< IEnumerable<ShopDbLib.Models.Model>> Get(int katalogId)
        {
             
          return  await _db.Model.Where(p=>p.KatalogId==katalogId).ToListAsync();
          
        }


           public async Task<bool> Add(ShopDbLib.Models.Model item){             
                
               // db.Users.Add(user);
             await _db.Model.AddAsync(item);  
              int i=  await _db.SaveChangesAsync() ;
            
            //  Console.WriteLine("async Task<bool> Add(Katalog item)-----------"+i.ToString()+"_db.Entry.State--"+_db.Entry(item).State.ToString());

              if(i!=0)   return true;
              else return false;          
                
         }

            public async Task< bool> Update(ShopDbLib.Models.Model item){
                     
             ShopDbLib.Models.Model selectItem= await _db.Model.FirstOrDefaultAsync(x=>x.Id==item.Id);
            
             
               if (selectItem==null)
            {
                return false;
            }
       
                if(selectItem.Name.Trim()==item.Name.Trim())
                {
          
                    return true;
                }
                selectItem.Name=item.Name;
 //throw new Exception("not implimetn exeption 18.11.20");
                
           _db.Model.Update(selectItem);
        
         int i=   await _db.SaveChangesAsync();
           if(i!=0){
               return true;
           }
           return false;
               
          }

    }
}