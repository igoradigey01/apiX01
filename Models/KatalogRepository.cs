using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using  Microsoft.EntityFrameworkCore;

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

        public async Task< IEnumerable<Katalog>> Get()
        {
             
          //  throw new Exception("not implimetn exeption 14.11.20");
          return await _db.Katalog.ToListAsync();
        }

         public async Task<bool> Add(Katalog item){             
                
               // db.Users.Add(user);
             await _db.Katalog.AddAsync(item);  
              int i=  await _db.SaveChangesAsync() ;
            
            //  Console.WriteLine("async Task<bool> Add(Katalog item)-----------"+i.ToString()+"_db.Entry.State--"+_db.Entry(item).State.ToString());

              if(i!=0)   return true;
              else return false;          
                
         }

          public async Task< bool> Update(Katalog item){
             //  throw new Exception("NOt Implimetn Exception");
           //   Console.WriteLine("async Task<bool> Update(Katalog item)------_db.Kagalog.Any-----"+item.Id.ToString()+" "+item.Name);
             
             Katalog selectItem= await _db.Katalog.FirstOrDefaultAsync(x=>x.Id==item.Id);
            
             
               if (selectItem==null)
            {
                return false;
            }
       //   Console.WriteLine("async Task<bool> Update(Katalog item)-----------"+item.Id.ToString()+" "+item.Name);
                if(selectItem.Name.Trim()==item.Name.Trim())
                {
              //      Console.WriteLine("async Task<bool> Update(Katalog item)-if(n==name)----------"+item.Id.ToString()+" "+item.Name);
                    return true;
                }
                selectItem.Name=item.Name;
                
           _db.Katalog.Update(selectItem);
        
         int i=   await _db.SaveChangesAsync();
           if(i!=0){
               return true;
           }
           return false;
               
          }

          public async Task< bool> Delete(int id){
             
              
                Katalog item = _db.Katalog.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return false;
            }
            _db.Katalog.Remove(item);
           int i= await _db.SaveChangesAsync();
           if(i!=0) return true;
           else return false;              
            
          }
        
    }

}