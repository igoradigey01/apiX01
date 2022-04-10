using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopDb;

namespace ShopAPI.Model
{
    public class KatalogNRepository
    {
        private readonly MyShopDbContext _db;

        public KatalogNRepository(MyShopDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<KatalogN>> Get()
        {
            //  throw new Exception("not implimetn exeption 14.11.20");
            return await _db.KatalogNs.ToListAsync();
        }

        public async Task<IEnumerable<KatalogN>> GetPostavchik(int idPostavchik)
        {
            return await _db.KatalogNs.Where(k=>k.PostavchikId == idPostavchik).ToListAsync();
        }

        public async Task<KatalogN> Item(int idKatalogN)
        {

            return await _db.KatalogNs.SingleOrDefaultAsync(c => c.Id == idKatalogN);
        }

        public async Task<IEnumerable<KatalogN>> KatalogNs(int idCategoriaN)
        {

            return await _db.KatalogNs.Where(d => d.CategoriaId == idCategoriaN).ToListAsync();

        }

        public async Task<DtoRepositoryResponse> Create(KatalogN item)
        {
            DtoRepositoryResponse flag = new DtoRepositoryResponse { Flag = false, Message = null };
            await _db.KatalogNs.AddAsync(item);
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

        public async Task<DtoRepositoryResponse> Update(KatalogN item)
        {
            DtoRepositoryResponse flag = new DtoRepositoryResponse { Flag = false, Message = null, Item = null };
            KatalogN selectItem = await _db.KatalogNs.FirstOrDefaultAsync(c => c.Id == item.Id);
            if (selectItem == null)
            {

                flag.Message = "Товар с таким id  в БД ненайден";
                return flag;

            }
            selectItem.Name = item.Name.Trim();//23.02.22

            selectItem.Hidden = item.Hidden;

            selectItem.CategoriaId = item.CategoriaId;
            selectItem.PostavchikId = item.PostavchikId;
            selectItem.DecriptSEO = item.DecriptSEO;

            _db.KatalogNs.Update(selectItem);

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
            KatalogN item = await _db.KatalogNs.FirstOrDefaultAsync(c => c.Id == id);
            if (item == null)
            {
                flagValid.Message = "Каталога с таким id не существует!";
                flagValid.Flag = false;
                return flagValid;
            }
            _db.KatalogNs.Remove(item);
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
