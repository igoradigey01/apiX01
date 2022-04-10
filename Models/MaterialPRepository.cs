using Microsoft.EntityFrameworkCore;
using ShopDb;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopAPI.Model
{
    // смотри---ClassData.cs-- :ICRUD named- Iterfaise
    public class MaterialPRepository
    {
        private readonly MyShopDbContext _db;


        public MaterialPRepository
        (
            MyShopDbContext db
        )
        {
            _db = db;

        }

        public async Task<IEnumerable<MaterialP>> Get()
        {
            //  throw new Exception("not implimetn exeption 14.11.20");
            return await _db.MaterialPs.ToListAsync();
        }

        public async Task<MaterialP> Item(int idMaterial){

              return await _db.MaterialPs.SingleOrDefaultAsync(c => c.Id == idMaterial);
        }

        

        public async Task<DtoRepositoryResponse> Create(MaterialP item)
        {
            DtoRepositoryResponse flag = new DtoRepositoryResponse { Flag = false, Message = null };
            await _db.MaterialPs.AddAsync(item);
            int i = await _db.SaveChangesAsync();

            //  Console.WriteLine("async Task<bool> Add(Katalog item)-----------"+i.ToString()+"_db.Entry.State--"+_db.Entry(item).State.ToString());

            if (i != 0)
            {
                flag.Flag = true;
                flag.Message = "БД add ok!";
                flag.Item= item;
                return flag;
            }
            else
            {
                flag.Flag = false;
                flag.Message = "БД add not!";
                return flag;
            }

        }
      

        public async Task<DtoRepositoryResponse> Update(MaterialP item)
        {
            DtoRepositoryResponse flag = new DtoRepositoryResponse { Flag = false, Message = null, Item = null };
            MaterialP selectItem= await _db.MaterialPs.FirstOrDefaultAsync(c => c.Id == item.Id);
            if (selectItem == null)
            {

                flag.Message = "Товар с таким id  в БД ненайден";
                return flag;

            }
            selectItem.Name = item.Name.Trim();//23.02.22
           
            selectItem.Hidden = item.Hidden;
            selectItem.Description = item.Description;
            _db.MaterialPs.Update(selectItem);

            int i = await _db.SaveChangesAsync();
            if (i != 0)
            {
                flag.Message = "БД update ok!";
                flag.Flag = true;
                flag.Item = selectItem;
                return flag;
            }
            else
            {
                flag.Message = "БД update not(false)!";
                flag.Flag = false;
                flag.Item = selectItem;
                return flag;
            }




        }

        

        public async Task<DtoRepositoryResponse> Delete(int id)
        {
            DtoRepositoryResponse flagValid = new DtoRepositoryResponse { Flag = false, Message = "" };
            MaterialP item = await _db.MaterialPs.FirstOrDefaultAsync(c => c.Id == id);
            if (item == null)
            {
                flagValid.Message = "Каталога с таким id не существует!";
                flagValid.Flag = false;
                return flagValid;
            }
            _db.MaterialPs.Remove(item);
            int i = await _db.SaveChangesAsync();
            if (i != 0)
            {
                flagValid.Flag = true;
                flagValid.Message = "БД delete ok";
                return flagValid;
            }
            else
            {
                flagValid.Message = "БД delete not";
                flagValid.Flag = false;
                return flagValid;
            }

        }

        
    }
}