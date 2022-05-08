using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopDb;

namespace ShopAPI.Model
{
    public class NomenclatureRepository
    {
        private readonly MyShopDbContext _db;

        public NomenclatureRepository(MyShopDbContext db)
        {
            _db = db;
        }

       /* public async Task<IEnumerable<Nomenclature>> Get()
        {
            //  throw new Exception("not implimetn exeption 14.11.20");
            return await _db.Nomenclatures.ToListAsync();
        }*/

        public async Task<IEnumerable<Nomenclature>> GetNomenclatures(int idPostavchik)
        {
            //  throw new Exception("not implimetn exeption 14.11.20");
            return await _db.Nomenclatures.Where(n=>n.PostavchikId==idPostavchik).ToListAsync();
        }

        public async Task<IEnumerable<Nomenclature>> GetNomenclatures(int idKatalog, int idPostavchik)
        {
            //  throw new Exception("not implimetn exeption 14.11.20");
            return await _db.Nomenclatures.Where(n => n.PostavchikId == idPostavchik&&n.KatalogId==idKatalog).ToListAsync();
        }

        public async Task<Nomenclature> Item(int idNomenclature)
        {

            return await _db.Nomenclatures.SingleOrDefaultAsync(c => c.Id == idNomenclature);
        }

        public async Task<DtoRepositoryResponse> Create(Nomenclature item)
        {
            DtoRepositoryResponse flag = new DtoRepositoryResponse { Flag = false, Message = null };
            await _db.Nomenclatures.AddAsync(item);
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

        public async Task<DtoRepositoryResponse> Update(Nomenclature item)
        {
            DtoRepositoryResponse flag = new DtoRepositoryResponse { Flag = false, Message = null, Item = null };
            Nomenclature selectItem = await _db.Nomenclatures.FirstOrDefaultAsync(c => c.Id == item.Id);
            if (selectItem == null)
            {

                flag.Message = "Товар с таким id  в БД ненайден";
                return flag;

            }
            selectItem.Name = item.Name.Trim();//23.02.22
          
            selectItem.Hidden = item.Hidden;           
            selectItem.InStock=  item.InStock ;
            selectItem.Sale= item.Sale;

            selectItem.Description = item.Description;
            selectItem.Price = item.Price;
            selectItem.Markup= item.Markup;
            selectItem.Position = item.Position;
            selectItem.ArticleId = item.ArticleId;
            selectItem.BrandId = item.BrandId;
            selectItem.ColorId = item.ColorId;
            selectItem.KatalogId = item.KatalogId;
            selectItem.PostavchikId= item.PostavchikId;
            selectItem.Guid= item.Guid;

            _db.Nomenclatures.Update(selectItem);

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

        public async Task<DtoRepositoryResponse> UpdateDataPrice(ShopAPI.Controllers.PriceN[] items)
        {
            DtoRepositoryResponse flag = new DtoRepositoryResponse { Flag = false, Message = null, Item = null };
         // Nomenclature[] nomenclatures; 
            foreach(var item in items) { 
            Nomenclature selectItem = await _db.Nomenclatures.FirstOrDefaultAsync(c => c.Id == item.Id);
            if (selectItem == null)
            {

                flag.Message = "Товар с таким id  в БД ненайден";
                return flag;

            }
              selectItem.Price = item.Price;

                _db.Nomenclatures.Update(selectItem);

                int i = await _db.SaveChangesAsync();

                if (i != 0)
                {

                    flag.Message = "БД update ok!";
                    flag.Flag = true;
                    flag.Item = null;
                    return flag;
                }
                else
                {
                    flag.Message = "Error!! Прайс обновлен не полностью!!";
                    flag.Flag = false;
                    flag.Item = selectItem;
                    return flag;
                }
            }
            flag.Message = "БД update false - что-то пошло не так!";
            flag.Flag = false;
            flag.Item = null;

            return flag;
        }

        public async Task<DtoRepositoryResponse> Delete(int id)
        {
            DtoRepositoryResponse flagValid = new DtoRepositoryResponse { Flag = false, Message = "" };
            Nomenclature item = await _db.Nomenclatures.FirstOrDefaultAsync(c => c.Id == id);
            if (item == null)
            {
                flagValid.Message = "Каталога с таким id не существует!";
                flagValid.Flag = false;
                return flagValid;
            }
            _db.Nomenclatures.Remove(item);
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
