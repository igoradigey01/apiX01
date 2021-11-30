using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopDb;
using Microsoft.AspNetCore.Http;

namespace ShopAPI.Model
{
    // смотри---ClassData.cs-- :ICRUD named- Iterfaise
    public class ProductRepository
    {
        private readonly MyShopContext _db;
        // public     delegate void  Save(string imgPath,IFormFile photo);
        public ProductRepository(MyShopContext db)
        {
            _db = db;
        }



        public async Task<IEnumerable<Product>> Get(int katalogId)
        {

            return await _db.Products.Where(p => p.KatalogId == katalogId).ToListAsync();

        }

        public async Task<Product> Item(int idProduct)
        {
            return await _db.Products.Where(p => p.Id == idProduct).FirstOrDefaultAsync();
        }

        public async Task<Product> ItemLoadChild(Product item)
        {
            _db.Attach<Product>(item);
            await _db.Entry<Product>(item).Collection<Image>(p => p.Images).LoadAsync();
            return item;
        }

        public async Task<RepositoryResponseDto > Create(Product item)
        {
            var flag = new RepositoryResponseDto  { Flag = false, Item = null, Message = "" };
            // db.Users.Add(user);
            if (item.KatalogId == -1)
            {
                flag.Message = "Ошибка БД  KatalogId==-1";
                flag.Flag = false;

                return flag;
            }
            if (item.TypeProductId == -1)
            {
                flag.Message = "Ошибка БД TypeProductId==-1";
                flag.Flag = false;
                return flag;
            }
            // Проверить на уникольность ???

            await _db.Products.AddAsync(item);
            int i = await _db.SaveChangesAsync();

            //  Console.WriteLine("async Task<bool> Add(Katalog item)-----------"+i.ToString()+"_db.Entry.State--"+_db.Entry(item).State.ToString());

            if (i != 0)
            {
                ///  Console.WriteLine("async Task<bool> Add(Katalog item)--- _del.Invoke-- bigin"+"_dev==null"+(_del==null).ToString());
                // _del(selectItem.Image,photo); делегат не работаете  error null async metod
                //   Console.WriteLine("async Task<bool> Add(Katalog item)--- _del.Invoke-- end");
                flag.Message = "";
                flag.Flag = true;
                return flag;
            }
            flag.Message = "Ошибка субд Create,запись в бд не создана!";
            flag.Flag = false;
            return flag;

        }
        //Обновляет значения в бд но не img (photo)
        public async Task<RepositoryResponseDto > Update(Product item)
        {
            bool flag_edit = false;

            Product selectItem = await _db.Products.FirstOrDefaultAsync(x => x.Id == item.Id);

            //   var flagValid=new FlagValid {Flag=false,ErrorMessage=""};
            var flag = new RepositoryResponseDto  { Flag = false, Item = null, Message = "" };


            if (selectItem == null)
            {
                return new RepositoryResponseDto  { Flag = false, Message = "Товар с таким id  в БД ненайден" };
            }
            // проверка на уникльность
            if (item.KatalogId == -1)
            {
                flag.Message = "Ошибка БД  KatalogId==-1";
                flag.Flag = false;

                return flag;
            }
            if (item.TypeProductId == -1)
            {
                flag.Message = "Ошибка БД TypeProductId==-1";
                flag.Flag = false;
                return flag;
            }


            if (selectItem.KatalogId != item.KatalogId)
            {
                flag_edit = true;
                selectItem.KatalogId = item.KatalogId;
            }

            if (selectItem.TypeProductId != item.TypeProductId)
            {
                flag_edit = true;
                selectItem.TypeProductId = item.TypeProductId;
            }

            if (selectItem.Name != item.Name.Trim())
            {
                flag_edit = true;
                selectItem.Name = item.Name.Trim();
            }
            if (selectItem.Description != item.Description)
            {
                flag_edit = true;
                selectItem.Description = item.Description;
            }
            if (selectItem.Price != item.Price)
            {
                flag_edit = true;
                selectItem.Price = item.Price;
            }
            if (selectItem.Markup != item.Markup)
            {
                flag_edit = true;
                selectItem.Markup = item.Markup;
            }
            selectItem.Image = item.Image;


            // _db.Product.Update(selectItem);

            //  _db.Product.Update(item);
            int i = await _db.SaveChangesAsync();
            if (i != 0)
            {
                flag.Message = "";
                flag.Flag = true;
                //    _del.Invoke(selectItem.Image,photo);
                return flag;
            }
            if (!flag_edit)
            {
                flag.Message = "";
                flag.Flag = true;
                //    _del.Invoke(selectItem.Image,photo);
                return flag;

            }
            flag.Flag = false;
            flag.Message = "Ошибка субд Update,запись в бд не создана!";
            return flag;

        }

        public async Task<RepositoryResponseDto > Delete(Product item)
        {

            RepositoryResponseDto  flag = new RepositoryResponseDto  { Flag = false, Message = null, Item = null };
            int i = 0;



            //  Product item = _db.Products.FirstOrDefault(x => x.Id == id);
            try
            {
                // Image item= await    _db.Images.Where(p=>p.Id==id).FirstOrDefaultAsync();
                _db.Attach<Product>(item);
                _db.Products.Remove(item);
                i = await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("--- ProductRepository----Ошибка БД Images delete not(false)!");
                Console.WriteLine(ex.Message);
                flag.Message = "БД Products delete not(false)!";
                flag.Flag = false;
            }
            if (i != 0)
            {
                flag.Message = "БД Products delete ok!";
                flag.Flag = true;
                flag.Item = null;
                return flag;
            }
            else
            {
                flag.Message = "БД Products delete not(false)!";
                flag.Flag = false;
                flag.Item = null;
                return flag;
            }

        }

        public async Task<RepositoryResponseDto > DeleteChildImage(Image item)
        {
            throw new Exception("NOt Implimetn Exception");

        }
        public bool NameUnique(string name)
        {
            var selectItem = _db.Products.FirstOrDefault(x => x.Name == name);
            // Console.WriteLine((selectItem == null ).ToString()+"selectItem ==null");            

            if (selectItem != null) return false;
            return true;


        }






    }
}