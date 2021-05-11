using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopDb;
using Microsoft.AspNetCore.Http;

namespace WebShopAPI.Model
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

        public async Task<Product> Item(int idProduct){
            return await _db.Products.Where(p=>p.Id==idProduct).FirstOrDefaultAsync();
        }

        public async Task<Product>ItemLoadChild(Product item){
            _db.Attach<Product>(item);
         await   _db.Entry<Product>(item).Collection<Image>(p=>p.Images).LoadAsync();
            return item ;
        }

              public async Task<bool> Create(Product item)
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

            await _db.Products.AddAsync(item);
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

            Product selectItem = await _db.Products.FirstOrDefaultAsync(x => x.Id == item.Id);

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

        public async Task<FlagValid> Delete(Product item)
        {
            
               FlagValid  flag=new FlagValid{Flag=false,Message=null,Item=null};
               int i=0;



          //  Product item = _db.Products.FirstOrDefault(x => x.Id == id);
            try{
           // Image item= await    _db.Images.Where(p=>p.Id==id).FirstOrDefaultAsync();
           _db.Attach<Product>(item);
           _db.Products.Remove(item);
              i=await  _db.SaveChangesAsync();
               }
              catch (Exception ex)
            {
                Console.WriteLine("--- ProductRepository----Ошибка БД Images delete not(false)!");
                Console.WriteLine(ex.Message);
                 flag.Message="БД Products delete not(false)!";
                  flag.Flag=false;
            }
            if(i!=0){
              flag.Message="БД Products delete ok!";
              flag.Flag=true;
              flag.Item=null;
               return flag;
           }
          else {
              flag.Message="БД Products delete not(false)!";
              flag.Flag=false;
              flag.Item=null;
               return flag;
           }

        }

        public async Task<FlagValid> DeleteChildImage(Image item){
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