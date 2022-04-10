using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopDb;

namespace ShopAPI.Model
{
    public class ColorNRepository
    {
        private readonly MyShopDbContext _db;

        public ColorNRepository(MyShopDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ColorN>> Get()
        {
            //  throw new Exception("not implimetn exeption 14.11.20");
            return await _db.ColorNs.ToListAsync();
        }

        public async Task<IEnumerable<ColorN>> GetPostavchik(int idPostavchik)
        {
            return await _db.ColorNs.Where(c=>c.PostavchikId == idPostavchik).ToListAsync();
        }

        public async Task<ColorN> Item(int idColor)
        {

            return await _db.ColorNs.SingleOrDefaultAsync(c => c.Id == idColor);
        }

        public async Task<DtoRepositoryResponse> Create(ColorN item)
        {
            DtoRepositoryResponse flag = new DtoRepositoryResponse { Flag = false, Message = null };
            await _db.ColorNs.AddAsync(item);
            int i = await _db.SaveChangesAsync();

            //  Console.WriteLine("async Task<bool> Add(Katalog item)-----------"+i.ToString()+"_db.Entry.State--"+_db.Entry(item).State.ToString());

            if (i != 0)
            {
                flag.Flag = true;
                flag.Message = "БД add ok!";
                flag.Item = item;
                return flag;
            }
            else
            {
                flag.Flag = false;
                flag.Message = "БД add not!";
                return flag;
            }

        }

        public async Task<DtoRepositoryResponse> Update(ColorN item)
        {
            DtoRepositoryResponse flag = new DtoRepositoryResponse { Flag = false, Message = null, Item = null };
            ColorN selectItem = await _db.ColorNs.FirstOrDefaultAsync(c => c.Id == item.Id);
            if (selectItem == null)
            {

                flag.Message = "Товар с таким id  в БД ненайден";
                return flag;

            }
            selectItem.Name = item.Name.Trim();//23.02.22

            selectItem.Hidden = item.Hidden;
            selectItem.PostavchikId = item.PostavchikId;

            _db.ColorNs.Update(selectItem);

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
            ColorN item = await _db.ColorNs.FirstOrDefaultAsync(c => c.Id == id);
            if (item == null)
            {
                flagValid.Message = "Каталога с таким id не существует!";
                flagValid.Flag = false;
                return flagValid;
            }
            _db.ColorNs.Remove(item);
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
