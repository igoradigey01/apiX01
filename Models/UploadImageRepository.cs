using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using ImageMagick;

namespace WebShopAPI.Model
{
    public interface IUploadFile
    {
        public void Save(string imgPath, IFormFile photo);
        public void Update(string imgPath, IFormFile photo);
        public void Delete(string imgPath);

    }

    //------------------------------------

    public class UploadImageRepository : IUploadFile
    {

        private readonly string _imgDir;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UploadImageRepository(IConfiguration config, IWebHostEnvironment environment)
        {
            _imgDir = config["ImagesDir"];
            _hostingEnvironment = environment;

        }

        public void Save(string imgPath, IFormFile photo)
        {
            /// релализовать обработку err при записи файла using(){}??
            // throw new Exception("NOt Implimetn Exception");
            try
            {
                SaveImgFile(imgPath, photo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка  imgSave");
                Console.WriteLine(ex.Message);
            }
        }
        public void Update(string imgPath, IFormFile photo)
        {
            try
            {
               SaveImgFile(imgPath, photo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка Обновления imgUpdate");
                Console.WriteLine(ex.Message);
            }
        }
        public void Delete(string imgPath)
        {
            throw new Exception("NOt Implimetn Exception");
        }

        public string GetImgPathNewName(string imgName)
        {
            string rootwww = _hostingEnvironment.WebRootPath;
            var extenion = System.IO.Path.GetExtension(imgName);
            var name = Guid.NewGuid().ToString();
            Console.WriteLine("Guid file name --" + name);

            string filePath = System.IO.Path.Combine(rootwww, _imgDir, name + extenion);
            Console.WriteLine("filePath --" + filePath);
            return filePath;
        }
        // old vershion not resaze file img- 28.03.21
        private async void WriteOldFile(string pathPhoto, IFormFile filePhoto)
        {
            if (filePhoto != null)
            {
                // путь к папке Files

                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(pathPhoto, FileMode.Truncate))
                {
                    await filePhoto.CopyToAsync(fileStream);
                }

            }
            else
            {
                throw new Exception("IFormFile filePhoto==null Exception");

            }

        }
        // old vershion not resaze file img -28.03.21
        private async void WriteNewFile(string pathPhoto, IFormFile filePhoto)
        {
            //    throw new Exception("NOt Implimetn Exception");

            Console.WriteLine("WriteNewFile(string pathPhoto,IFormFile filePhoto --pathPhoto--" + pathPhoto);
            if (filePhoto != null)
            {
                // путь к папке Files

                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(pathPhoto, FileMode.Create))
                {
                    await filePhoto.CopyToAsync(fileStream);
                }

            }
            else
            {
                throw new Exception("IFormFile filePhoto==null Exception");

            }

        }
        // new vershion -resaze file img -28.03.21
        private void SaveImgFile(string pathPhoto, IFormFile filePhoto)
        {

            const int width = 600;
            const int quality = 72;

            using (var image = new MagickImage(filePhoto.OpenReadStream()))
            {
                //image.Resize()
                var height = image.Width / image.Height * width;
                image.Resize(width, height);
                image.Strip();
                image.Quality = quality;
                image.Write(pathPhoto);

            }
        }

    }
}