using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopDb;

namespace ShopAPI.Model
{
    public class ArticleNRepository
    {
        private readonly MyShopDbContext _db;

        public ArticleNRepository(MyShopDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ArticleN>> Get()
        {
            //  throw new Exception("not implimetn exeption 14.11.20");
            return await _db.ArticleNs.ToListAsync();
        }

        public async Task<IEnumerable<ArticleN>> GetPostavchik(int idPostavchik)
        {
            return await _db.ArticleNs.Where(a=>a.PostavchikId == idPostavchik).ToListAsync(); 
        }

        public async Task<ArticleN> Item(int idArticle)
        {

            return await _db.ArticleNs.SingleOrDefaultAsync(c => c.Id == idArticle);
        }

        public async Task<DtoRepositoryResponse> Create(ArticleN item)
        {
            DtoRepositoryResponse flag = new DtoRepositoryResponse { Flag = false, Message = null };
            await _db.ArticleNs.AddAsync(item);
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

        public async Task<DtoRepositoryResponse> Update(ArticleN item)
        {
            DtoRepositoryResponse flag = new DtoRepositoryResponse { Flag = false, Message = null, Item = null };
            ArticleN selectItem = await _db.ArticleNs.FirstOrDefaultAsync(c => c.Id == item.Id);
            if (selectItem == null)
            {

                flag.Message = "Товар с таким id  в БД ненайден";
                return flag;

            }
            selectItem.Name = item.Name.Trim();//23.02.22
            selectItem.PostavchikId = item.PostavchikId;

            selectItem.Hidden = item.Hidden;

            _db.ArticleNs.Update(selectItem);

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
            ArticleN item = await _db.ArticleNs.FirstOrDefaultAsync(c => c.Id == id);
            if (item == null)
            {
                flagValid.Message = "Каталога с таким id не существует!";
                flagValid.Flag = false;
                return flagValid;
            }
            _db.ArticleNs.Remove(item);
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
