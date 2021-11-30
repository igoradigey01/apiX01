using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using ShopDb;

namespace ShopAPI.Model
{
    // смотри---ClassData.cs-- :ICRUD named- Iterfaise
    public class TypeProductRepository
    {
        private readonly MyShopContext _db;


        public TypeProductRepository
        (
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

        public async Task<TypeProduct>Item(int id){

               throw new Exception("not implimetn exeption 14.11.20");
        }

        public async Task<RepositoryResponseDto > Create(TypeProduct typeProduct)
        {
            throw new Exception("not implimetn exeption 14.11.20");
            // return await _db.TypeProduct.ToListAsync();
        }
        public async Task<RepositoryResponseDto > Update(TypeProduct typeProduct)
        {
            throw new Exception("not implimetn exeption 14.11.20");
            // return await _db.TypeProduct.ToListAsync();
        }
        public async Task<RepositoryResponseDto > Delete(int id)
        {
            throw new Exception("not implimetn exeption 14.11.20");
            // return await _db.TypeProduct.ToListAsync();
        }
    }
}