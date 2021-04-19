using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopDbLibNew;
using Microsoft.AspNetCore.Http;

namespace WebShopAPI.Model
{
    public class ProductRepository
    {
        private readonly MyShopContext _db;
        // public     delegate void  Save(string imgPath,IFormFile photo);
        public ProductRepository(MyShopContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ShopDbLibNew.Product>> Get()
        {

            //  throw new Exception("not implimetn exeption 14.11.20");
            return await _db.Product.ToListAsync();

        }

        public async Task<IEnumerable<Product>> Get(int katalogId)
        {

            return await _db.Product.Where(p => p.KatalogId == katalogId).ToListAsync();

        }

        public bool NameUnique(string name)
        {
            var selectItem = _db.Product.FirstOrDefault(x => x.Name == name);
            // Console.WriteLine((selectItem == null ).ToString()+"selectItem ==null");            

            if (selectItem != null) return false;
            return true;


        }


        public async Task<bool> Add(Product item)
        {

            // db.Users.Add(user);
            if (item.KatalogId == -1)
            {
                return false;
            }
            if (item.TypeProductId == -1)
            {
                return false;
            }
            // Проверить на уникольность ???

            await _db.Product.AddAsync(item);
            int i = await _db.SaveChangesAsync();

            //  Console.WriteLine("async Task<bool> Add(Katalog item)-----------"+i.ToString()+"_db.Entry.State--"+_db.Entry(item).State.ToString());

            if (i != 0)
            {
                ///  Console.WriteLine("async Task<bool> Add(Katalog item)--- _del.Invoke-- bigin"+"_dev==null"+(_del==null).ToString());
                // _del(selectItem.Image,photo); делегат не работаете  error null async metod
                //   Console.WriteLine("async Task<bool> Add(Katalog item)--- _del.Invoke-- end");
                return true;
            }
            else return false;

        }
        //Обновляет значения в бд но не img (photo)



        public async Task<FlagValid> Update(Product item)
        {

            Product selectItem = await _db.Product.FirstOrDefaultAsync(x => x.Id == item.Id);

            //   var flagValid=new FlagValid {Flag=false,ErrorMessage=""};


            if (selectItem == null)
            {
                return new FlagValid { Flag = false, Message = "Товар с таким id  в БД ненайден" };
            }
            // проверка на уникльность
           
            

            if (selectItem.KatalogId != item.KatalogId)
            {
                selectItem.KatalogId = item.KatalogId;
            }

            if (selectItem.TypeProductId != item.TypeProductId)
            {
                selectItem.TypeProductId = item.TypeProductId;
            }



            selectItem.Name = item.Name.Trim();
            selectItem.Description = item.Description;
            if (selectItem.Price != item.Price)
            {
                selectItem.Price = item.Price;
            }
            if (selectItem.Markup != item.Markup)
            {
                selectItem.Markup = item.Markup;
            }
            selectItem.Image = item.Image;

             
          // _db.Product.Update(selectItem);
         
          //  _db.Product.Update(item);
            int i = await _db.SaveChangesAsync();
            if (i != 0)
            {
                //    _del.Invoke(selectItem.Image,photo);
                return new FlagValid{Flag=true,Message=""};
            }
            return new FlagValid{Flag=false,Message="Ошибка SaveChangesAsync() БД поле Prodcut not Update"};

        }

        public async Task<bool> Delete(int id)
        {


            Product item = _db.Product.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return false;
            }
            _db.Product.Remove(item);
            int i = await _db.SaveChangesAsync();
            if (i != 0) return true;
            else return false;

        }

    }
}