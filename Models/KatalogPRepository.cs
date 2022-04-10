using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using  Microsoft.EntityFrameworkCore;

using ShopDb;

namespace ShopAPI.Model
{
    public class KatalogPRepository
    {
        private readonly MyShopDbContext _db;
       

        public KatalogPRepository(
            MyShopDbContext db
            
            )
        {
            _db = db;
           
        }

        public async Task< IEnumerable<KatalogP>> Get()
        {
             
          //  throw new Exception("not implimetn exeption 14.11.20");
          return await _db.KatalogPs.ToListAsync();
        }

        public async Task<IEnumerable<Product>> Get(int katalogId)
        {

            return await _db.Products.Where(p => p.KatalogId == katalogId).ToListAsync();

        }

        public async Task<DtoRepositoryResponse > Add(KatalogP item){             
                DtoRepositoryResponse  flag=new DtoRepositoryResponse {Flag=false,Message=null};
               // db.Users.Add(user);
             await _db.KatalogPs.AddAsync(item);  
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

          public async Task< DtoRepositoryResponse > Update(KatalogP item){
             //  throw new Exception("NOt Implimetn Exception");
           //   Console.WriteLine("async Task<bool> Update(Katalog item)------_db.Kagalog.Any-----"+item.Id.ToString()+" "+item.Name);
             DtoRepositoryResponse   flag=new DtoRepositoryResponse {Flag=false,Message=null,Item=null};
             KatalogP selectItem= await _db.KatalogPs.FirstOrDefaultAsync(x=>x.Id==item.Id);
            
             
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

                
           _db.KatalogPs.Update(selectItem);
        
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

          public async Task< DtoRepositoryResponse > Delete(int id){
             DtoRepositoryResponse  flagValid=new DtoRepositoryResponse {Flag=false,Message=""};
              
                KatalogP item = await _db.KatalogPs.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
              flagValid.Message="Каталога с таким id не существует!";
              flagValid.Flag=false;
                return flagValid;
            }
            _db.KatalogPs.Remove(item);
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