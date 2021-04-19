using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using  Microsoft.EntityFrameworkCore;

using ShopDbLibNew;

namespace WebShopAPI.Model
{
    public class KatalogRepository
    {
        private readonly MyShopContext _db;
       

        public KatalogRepository(
            MyShopContext db
            
            )
        {
            _db = db;
           
        }

        public async Task< IEnumerable<Katalog>> Get()
        {
             
          //  throw new Exception("not implimetn exeption 14.11.20");
          return await _db.Katalog.ToListAsync();
        }

         public async Task<FlagValid> Add(Katalog item){             
                FlagValid flag=new FlagValid{Flag=false,Message=null};
               // db.Users.Add(user);
             await _db.Katalog.AddAsync(item);  
              int i=  await _db.SaveChangesAsync() ;
            
            //  Console.WriteLine("async Task<bool> Add(Katalog item)-----------"+i.ToString()+"_db.Entry.State--"+_db.Entry(item).State.ToString());

              if(i!=0){  
                flag.Flag=true;
                flag.Message="БД add ok!";
               return flag;
              }
              else {
                flag.Flag=false;
                flag.Message="БД add not!";
              return flag;         
              }
         }

          public async Task< FlagValid> Update(Katalog item){
             //  throw new Exception("NOt Implimetn Exception");
           //   Console.WriteLine("async Task<bool> Update(Katalog item)------_db.Kagalog.Any-----"+item.Id.ToString()+" "+item.Name);
             FlagValid  flag=new FlagValid{Flag=false,Message=null,Item=null};
             Katalog selectItem= await _db.Katalog.FirstOrDefaultAsync(x=>x.Id==item.Id);
            
             
               if (selectItem==null)
            {

                 flag.Message = "Товар с таким id  в БД ненайден";
                 return flag;
                 
            }
       
           selectItem.Name=item.Name.Trim();
                
           _db.Katalog.Update(selectItem);
        
         int i=   await _db.SaveChangesAsync();
           if(i!=0){
              flag.Message="БД update ok!";
              flag.Flag=true;
              flag.Item=selectItem;
               return flag;
           }
          else {
              flag.Message="БД update not(false)!";
              flag.Flag=false;
              flag.Item=selectItem;
               return flag;
           }
               
          }

          public async Task< FlagValid> Delete(int id){
             FlagValid flagValid=new FlagValid{Flag=false,Message=""};
              
                Katalog item = _db.Katalog.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
              flagValid.Message="Каталога с таким id не существует!";
              flagValid.Flag=false;
                return flagValid;
            }
            _db.Katalog.Remove(item);
           int i= await _db.SaveChangesAsync();
           if(i!=0) {
             flagValid.Flag=true;
             flagValid.Message="БД delete ok";
             return flagValid;
           }
           else{ 
             flagValid.Message="БД delete not";
             flagValid.Flag=false;
             return flagValid;              
           }
            
          }
        
    }

}