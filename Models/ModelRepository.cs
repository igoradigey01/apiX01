using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using  Microsoft.EntityFrameworkCore;
using ShopDbLib.Models;
using Microsoft.AspNetCore.Http;

namespace WebShopAPI.Model
{
    public class ModelRepository
    {
        private readonly AppDbContext _db;
       public     delegate void  Save(string imgPath,IFormFile photo);
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

      
      public async Task<bool> Add(ShopDbLib.Models.Model item,Save _del,IFormFile photo){             
                
               // db.Users.Add(user);
               if(item.KatalogId==-1){
                return false;
            }
               // Проверить на уникольность ???
             var selectItem = await      _db.Model.FirstOrDefaultAsync(x=>x.Name==item.Name);
             if(selectItem!=null) return false; 
             await _db.Model.AddAsync(item);  
              int i=  await _db.SaveChangesAsync() ;
            
            //  Console.WriteLine("async Task<bool> Add(Katalog item)-----------"+i.ToString()+"_db.Entry.State--"+_db.Entry(item).State.ToString());

              if(i!=0)  {
                 _del.Invoke(selectItem.Image,photo);
                 return true;}
              else return false;          
                
         }

      public async Task< bool> Update(ModelSerialize itemSerialize,Save _del,IFormFile photo){
                     
             ShopDbLib.Models.Model selectItem= await _db.Model.FirstOrDefaultAsync(x=>x.Id==itemSerialize.Id);
          
          
            
             
               if (selectItem==null)
            {
                return false;
            }
                 // проверка на уникльность
              var selectUniItem = await      _db.Model.FirstOrDefaultAsync(x=>x.Name==itemSerialize.Name);
             if(selectUniItem!=null) return false; 
             
            
           
            if(selectItem.KatalogId!=itemSerialize.IdKatalog){
              selectItem.KatalogId=itemSerialize.IdKatalog;
            }

             

             if(selectItem.Name.Trim()!=itemSerialize.Name.Trim()){
               selectItem.Name=itemSerialize.Name.Trim();
             }
             if(selectItem.Price!=itemSerialize.Price){
               selectItem.Price=itemSerialize.Price;
             }

            

            
                
 //throw new Exception("not implimetn exeption 18.11.20");
                       
                
           _db.Model.Update(selectItem);
        
         int i=   await _db.SaveChangesAsync();
           if(i!=0){
             _del.Invoke(selectItem.Image,photo);
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