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
        public void Save(string imgPath, byte[] photo);
        public void Update(string imgPath, byte[] photo);
        public void Delete(string imgPath);

    }

    //------------------------------------

    public class ImageRepository : IUploadFile
    {

        private readonly string _imgDir;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ImageRepository(IConfiguration config, IWebHostEnvironment environment)
        {
            _imgDir = config["ImagesDir"];
            _hostingEnvironment = environment;

        }

        public void Save(string imgPath, byte[] photo)
        {
            /// релализовать обработку err при записи файла using(){}??
            // throw new Exception("NOt Implimetn Exception");
            try
            {
                //  SaveImgFile(imgPath, photo);
                WriteNewFile(imgPath, photo); //30.12.21 not resize file writer
            }
            catch (Exception ex)
            {
                Console.WriteLine($"-----UploadImageRepository--Ошибка---(Add)--img Save--{imgPath}--");
                Console.WriteLine(ex.Message);
            }
        }
        // for --test---------------
        public void SaveBase64Img(string imgPath, string imgBase64String)
        {
            using (StreamWriter sw = File.CreateText(imgPath))
            {
                sw.Write(imgBase64String);
                sw.Flush();
            }

        }
        public void Update(string imgPath, byte[] photo)
        {
            try
            {
               WriteOldFile(imgPath,photo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--UploadImageRepository-----Ошибка Обновления {imgPath}");
                Console.WriteLine(ex.Message);
            }
        }
        public void Delete(string imgPath)
        {   try
            {
            if (File.Exists(imgPath)){
              File.Delete(imgPath);
            }
             }
            catch (Exception ex)
            {
                Console.WriteLine($"---UploadImageRepository----Ошибка Delete file {imgPath}");
                Console.WriteLine(ex.Message);
            }
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
        private async void WriteOldFile(string pathPhoto, byte[] img)
        {
            // сохраняем файл в папку Files в каталоге wwwroot
            using (var fileStream = new FileStream(pathPhoto, FileMode.Truncate))
            {
                await fileStream.WriteAsync(img);
                await fileStream.FlushAsync();
            }

        }
        // old vershion not resaze file img -28.03.21
        private void WriteNewFile(string pathPhoto, byte[] img)
        {              // сохраняем файл в папку Files в каталоге wwwroot
            using (FileStream f = File.Create(pathPhoto))
            {

                f.WriteAsync(img);
                f.FlushAsync();
            }


        }

        // new vershion -resaze file img -28.03.21 Resaze file
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