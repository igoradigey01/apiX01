using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using  Microsoft.EntityFrameworkCore;
using ShopDbLibNew;
using Microsoft.AspNetCore.Http;

namespace WebShopAPI.Model
{
    public class ProductRepository
    {
        private readonly MyShopContext _db;
      // public     delegate void  Save(string imgPath,IFormFile photo);
        public ProductRepository( MyShopContext db )
        {
            _db = db;
        }

      public async Task< IEnumerable<ShopDbLibNew.Product>> Get()
        {
             
          //  throw new Exception("not implimetn exeption 14.11.20");
          return  await _db.Product.ToListAsync();
          
        }

      public async Task< IEnumerable<Product>> Get(int katalogId)
        {
             
          return  await _db.Product.Where(p=>p.KatalogId==katalogId).ToListAsync();
          
        }

        public bool NameUnique(string name){
           var selectItem =     _db.Product.FirstOrDefault(x=>x.Name==name);
            // Console.WriteLine((selectItem == null ).ToString()+"selectItem ==null");            
             
              if(selectItem!=null) return false; 
              return true;


        }

      
      public async Task<bool> Add(Product item){             
                
               // db.Users.Add(user);
               if(item.KatalogId==-1){
                return false;
            }
               // Проверить на уникольность ???
            
             await _db.Product.AddAsync(item);  
              int i=  await _db.SaveChangesAsync() ;
            
            //  Console.WriteLine("async Task<bool> Add(Katalog item)-----------"+i.ToString()+"_db.Entry.State--"+_db.Entry(item).State.ToString());

              if(i!=0)  {
              ///  Console.WriteLine("async Task<bool> Add(Katalog item)--- _del.Invoke-- bigin"+"_dev==null"+(_del==null).ToString());
             // _del(selectItem.Image,photo); делегат не работаете  error null async metod
              //   Console.WriteLine("async Task<bool> Add(Katalog item)--- _del.Invoke-- end");
                 return true;}
              else return false;          
                
         }

      public async Task< bool> Update(ModelSerialize itemSerialize,IFormFile photo){
                     
            Product selectItem= await _db.Product.FirstOrDefaultAsync(x=>x.Id==itemSerialize.Id);
          
          
            
             
               if (selectItem==null)
            {
                return false;
            }
                 // проверка на уникльность
              var selectUniItem = await      _db.Product.FirstOrDefaultAsync(x=>x.Name==itemSerialize.Name);
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
                       
                
           _db.Product.Update(selectItem);
        
         int i=   await _db.SaveChangesAsync();
           if(i!=0){
         //    _del.Invoke(selectItem.Image,photo);
               return true;
           }
           return false;
               
          }

      public async Task< bool> Delete(int id){
             
              
               Product item = _db.Product.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return false;
            }
            _db.Product.Remove(item);
           int i= await _db.SaveChangesAsync();
           if(i!=0) return true;
           else return false;              
            
          }

    }
}