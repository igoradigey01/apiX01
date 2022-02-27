using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using  Microsoft.EntityFrameworkCore;

using ShopDb;

namespace ShopAPI.Model
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
          return await _db.Katalogs.ToListAsync();
        }

        public async Task<IEnumerable<Product>> Get(int katalogId)
        {

            return await _db.Products.Where(p => p.KatalogId == katalogId).ToListAsync();

        }

        public async Task<RepositoryResponseDto > Add(Katalog item){             
                RepositoryResponseDto  flag=new RepositoryResponseDto {Flag=false,Message=null};
               // db.Users.Add(user);
             await _db.Katalogs.AddAsync(item);  
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

          public async Task< RepositoryResponseDto > Update(Katalog item){
             //  throw new Exception("NOt Implimetn Exception");
           //   Console.WriteLine("async Task<bool> Update(Katalog item)------_db.Kagalog.Any-----"+item.Id.ToString()+" "+item.Name);
             RepositoryResponseDto   flag=new RepositoryResponseDto {Flag=false,Message=null,Item=null};
             Katalog selectItem= await _db.Katalogs.FirstOrDefaultAsync(x=>x.Id==item.Id);
            
             
               if (selectItem==null)
            {

                 flag.Message = "Товар с таким id  в БД ненайден";
                 return flag;
                 
            }
       
        selectItem.Name=item.Name.Trim();//23.02.22
            selectItem.Flag_href = item.Flag_href;
            selectItem.Flag_link = item.Flag_link;
            selectItem.Hidden= item.Hidden;
            selectItem.Link = item.Link;
            selectItem.DecriptSEO = item.DecriptSEO;
            selectItem.KeywordsSEO = item.KeywordsSEO;

                
           _db.Katalogs.Update(selectItem);
        
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

          public async Task< RepositoryResponseDto > Delete(int id){
             RepositoryResponseDto  flagValid=new RepositoryResponseDto {Flag=false,Message=""};
              
                Katalog item = await _db.Katalogs.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
              flagValid.Message="Каталога с таким id не существует!";
              flagValid.Flag=false;
                return flagValid;
            }
            _db.Katalogs.Remove(item);
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