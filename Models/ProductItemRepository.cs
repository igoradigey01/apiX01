using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using ShopDb;

namespace ShopAPI.Model
{     
    // смотри---ClassData.cs-- :ICRUD named- Iterfaise
    public class ProductItemRepository
    {

        private readonly MyShopDbContext _db;
        public ProductItemRepository(MyShopDbContext db)
        {

            _db = db;
        }

        public async Task<IEnumerable<ImageP>> GetImags(int idProduct)
        {
            //  throw new Exception("NOt Implimetn Exception");
            var img = _db.ImagePs.Where(p => p.ProductId == idProduct);
           

            return await img.ToArrayAsync();

        }

        public async Task<ImageP> GetItemImage(int idItem){
              
             

            return await _db.ImagePs.Where(p=>p.Id==idItem).FirstOrDefaultAsync();
        }

        public async Task<Product> GetItemProducts(int id){
            return await _db.Products.Where(p=>p.Id==id).FirstOrDefaultAsync();
        }
               //-------------------------------------
        public async Task<DtoRepositoryResponse > UpdateImage(ImageP item){
           
           throw new Exception("not implimetn exeption 14.11.20");
        }       
        public async Task<DtoRepositoryResponse > CreateImage(ImageP item)
        {
            await _db.ImagePs.AddAsync(item);
            int i = await _db.SaveChangesAsync();

            //  Console.WriteLine("async Task<bool> Add(Katalog item)-----------"+i.ToString()+"_db.Entry.State--"+_db.Entry(item).State.ToString());
            DtoRepositoryResponse   flag=new DtoRepositoryResponse {Flag=false,Message=null,Item=null};
           if(i!=0){
              flag.Message="БД Images add ok!";
              flag.Flag=true;
              flag.Item=item;
               return flag;
           }
          else {
              flag.Message="БД Images add not(false)!";
              flag.Flag=false;
              flag.Item=null;
               return flag;
           }
        }
        
        public async Task<DtoRepositoryResponse > DeleteImage(ImageP item)
        {
               DtoRepositoryResponse   flag=new DtoRepositoryResponse {Flag=false,Message=null,Item=null};
               int i=0;

               try{
           // Image item= await    _db.Images.Where(p=>p.Id==id).FirstOrDefaultAsync();
           _db.Attach<ImageP>(item);
           _db.ImagePs.Remove(item);
              i=await  _db.SaveChangesAsync();
               }
              catch (Exception ex)
            {
                Console.WriteLine("---UploadImageRepository----Ошибка БД Images delete not(false)!");
                Console.WriteLine(ex.Message);
                 flag.Message="БД Images delete not(false)!";
                  flag.Flag=false;
            }
            if(i!=0){
              flag.Message="БД Images delete ok!";
              flag.Flag=true;
              flag.Item=null;
               return flag;
           }
          else {
              flag.Message="БД Images delete not(false)!";
              flag.Flag=false;
              flag.Item=null;
               return flag;
           }

          //  throw new Exception("not implimetn exeption 14.11.20");
            // return await _db.TypeProduct.ToListAsync();
        }

        //---------------Nomenclature ----------------------
       
                public async Task<IEnumerable<Nomenclature>> GetItemNomenclatures(int idProduct)
        {
            var nomenclatures = _db.ProductNomenclatures.Where(p => p.ProductId == idProduct).Select(n => n.Nomenclature);
            return await nomenclatures.ToArrayAsync();

        }

       
        public async Task<bool> AddItemNomenclature(int idNomenclature)
        {
            throw new Exception("not implimetn exeption 14.11.20");
            // return await _db.TypeProduct.ToListAsync();
        }
        
         // Update  не нужно либо удалить либо добавить
        public async Task<bool> DeleteItemNomenclature(int id)
        {
            throw new Exception("not implimetn exeption 14.11.20");
            // return await _db.TypeProduct.ToListAsync();
        }
    }
}