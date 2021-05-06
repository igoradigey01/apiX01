using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using ShopDb;

namespace WebShopAPI.Model
{
    public class TypeProductRepository
    {
        private readonly MyShopContext _db;


        public TypeProductRepository(
            MyShopContext db
            )
        {
            _db = db;

        }

        public async Task<IEnumerable<TypeProduct>> Get()
        {
            //  throw new Exception("not implimetn exeption 14.11.20");
            return await _db.TypeProducts.ToListAsync();
        }

        public async Task<FlagValid> Add(TypeProduct typeProduct)
        {
            throw new Exception("not implimetn exeption 14.11.20");
            // return await _db.TypeProduct.ToListAsync();
        }
        public async Task<FlagValid> Update(TypeProduct typeProduct)
        {
            throw new Exception("not implimetn exeption 14.11.20");
            // return await _db.TypeProduct.ToListAsync();
        }
        public async Task<FlagValid> Delete(int id)
        {
            throw new Exception("not implimetn exeption 14.11.20");
            // return await _db.TypeProduct.ToListAsync();
        }
    }
}