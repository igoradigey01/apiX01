using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using ShopDbLibNew;

namespace WebShopAPI.Model
{
    public class ProductItemRepository
    {

        private readonly MyShopContext _db;
        public ProductItemRepository(MyShopContext db)
        {

            _db = db;
        }

        public async Task<string[]> GetImags(int idProduct)
        {
            //  throw new Exception("NOt Implimetn Exception");
            var imgPaths = _db.Image.Where(p => p.Id == idProduct).Select(i => i.Name);
            foreach (string i in imgPaths ){

                Console.WriteLine(i);
            }

            return await imgPaths.ToArrayAsync();

        }

        public async Task<IEnumerable<Nomenclature>> GetNomenclatures(int idProduct)
        {
            var nomenclatures = _db.ProductNomenclature.Where(p => p.ProductId == idProduct).Select(n => n.Nomenclature);
            return await nomenclatures.ToArrayAsync();

        }

       

        public async Task<FlagValid> AddImage(Image image)
        {
            throw new Exception("not implimetn exeption 14.11.20");
            // return await _db.TypeProduct.ToListAsync();
        }
        public async Task<FlagValid> UpdateImage(Image image)
        {
            throw new Exception("not implimetn exeption 14.11.20");
            // return await _db.TypeProduct.ToListAsync();
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