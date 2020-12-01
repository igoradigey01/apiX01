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
               if(item.KatalogId==null){
                return false;
            }
               // Проверить на уникольность ???
             var selectItem = await      _db.Model.FirstOrDefaultAsync(x=>x.Name==item.Name);
             if(selectItem!=null) return false; 
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
                 // проверка на уникльность
              var selectUniItem = await      _db.Model.FirstOrDefaultAsync(x=>x.Name==item.Name);
             if(selectUniItem!=null) return false; 
             
            if(   selectItem.Image!=item.Image)
            {
              selectItem.Image=item.Image;
            }
           
            if(selectItem.KatalogId!=item.KatalogId){
              selectItem.KatalogId=item.KatalogId;
            }

             selectItem.Katalog=item.Katalog;

             if(selectItem.Name.Trim()!=item.Name.Trim()){
               selectItem.Name=item.Name;
             }
             if(selectItem.Price!=item.Price){
               selectItem.Price=item.Price;
             }
             selectItem.Nomenclatura=item.Nomenclatura;
                
 //throw new Exception("not implimetn exeption 18.11.20");
                       
                
           _db.Model.Update(selectItem);
        
         int i=   await _db.SaveChangesAsync();
           if(i!=0){
               return true;
           }
           return false;
               
          }

      public async Task< bool> Delete(int id){
             
              
               ShopDbLib.Models.Model item = _db.Model.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return false;
            }
            _db.Model.Remove(item);
           int i= await _db.SaveChangesAsync();
           if(i!=0) return true;
           else return false;              
            
          }

    }
}