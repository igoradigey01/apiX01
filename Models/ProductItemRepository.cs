using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using ShopDb;

namespace WebShopAPI.Model
{
    public class ProductItemRepository
    {

        private readonly MyShopContext _db;
        public ProductItemRepository(MyShopContext db)
        {

            _db = db;
        }

        public async Task<IEnumerable<Image>> GetImags(int idProduct)
        {
            //  throw new Exception("NOt Implimetn Exception");
            var img = _db.Images.Where(p => p.ProductId == idProduct);
           

            return await img.ToArrayAsync();

        }

        public async Task<IEnumerable<Nomenclature>> GetNomenclatures(int idProduct)
        {
            var nomenclatures = _db.ProductNomenclatures.Where(p => p.ProductId == idProduct).Select(n => n.Nomenclature);
            return await nomenclatures.ToArrayAsync();

        }

       

        public async Task<FlagValid> AddImage(Image item)
        {
            await _db.Images.AddAsync(item);
            int i = await _db.SaveChangesAsync();

            //  Console.WriteLine("async Task<bool> Add(Katalog item)-----------"+i.ToString()+"_db.Entry.State--"+_db.Entry(item).State.ToString());
            FlagValid  flag=new FlagValid{Flag=false,Message=null,Item=null};
           if(i!=0){
              flag.Message="БД add ok!";
              flag.Flag=true;
              flag.Item=null;
               return flag;
           }
          else {
              flag.Message="БД add not(false)!";
              flag.Flag=false;
              flag.Item=null;
               return flag;
           }
        }
        
        public async Task<FlagValid> DeleteImage(int id)
        {
            throw new Exception("not implimetn exeption 14.11.20");
            // return await _db.TypeProduct.ToListAsync();
        }

        //---------------Nomenclature ----------------------
       

        public async Task<bool> AddNomenclature(int idNomenclature)
        {
            throw new Exception("not implimetn exeption 14.11.20");
            // return await _db.TypeProduct.ToListAsync();
        }
         // Update  не нужно либо удалить либо добавить
        public async Task<bool> DeleteNomenclature(int id)
        {
            throw new Exception("not implimetn exeption 14.11.20");
            // return await _db.TypeProduct.ToListAsync();
        }
    }
}