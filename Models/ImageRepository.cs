using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using ImageMagick;
using System.Threading.Tasks;


namespace WebShopAPI.Model
{
    //------------------------------------


    public class ImageRepository
    {

        private readonly string _imgDir;
        private readonly IWebHostEnvironment _hostingEnvironment;
        // dir host img wwwroot/images
        private string GetImgPaht
        {
            get
            {
                string wwwroot = _hostingEnvironment.WebRootPath;
                return System.IO.Path.Combine(wwwroot, _imgDir);
            }
        }
        // generete random guid name-------
        public string GetImgRamdomName(string imgName)
        {

            var extenion = System.IO.Path.GetExtension(imgName);
            var name = Guid.NewGuid().ToString();
            //  Console.WriteLine("Guid file name --" + name);

            string fileName = System.IO.Path.Combine(name + extenion);
            //Console.WriteLine("filePath --" + filePath);
            return fileName;
        }

        public byte[] GetImage(string name)
        {
            var path = System.IO.Path.Combine(GetImgPaht, name);

            FileInfo fileInf = new FileInfo(path);
            if (fileInf.Exists)
            {
                try
                {
                    return File.ReadAllBytes(path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("-----UploadImageRepository--Ошибка---GetImage()----");
                    Console.WriteLine(ex.Message);
                }
            }

            string not_found = "not_found.png";
            var path_not_found = Path.Combine(GetImgPaht, not_found);
            return File.ReadAllBytes(path_not_found);

        }


        public ImageRepository(IConfiguration config, IWebHostEnvironment environment)
        {
            _imgDir = config["ImagesDir"];
            _hostingEnvironment = environment;

        }

        public void Save(string imgName, byte[] photo)
        {

            var imgPath = System.IO.Path.Combine(GetImgPaht, imgName);

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
        //---------------------------------------Convertor Base64 to Blob img


        public void Update(string imgName, byte[] photo)
        {
            var imgPath = System.IO.Path.Combine(GetImgPaht, imgName);
            FileInfo fileInf = new FileInfo(imgPath);
            if (fileInf.Exists)
            {
                try
                {
                    WriteOldFile(imgPath, photo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--UploadImageRepository-----Ошибка Обновления {imgPath}");
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
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
        }

        //--------------------------

        public void Delete(string imgName)
        {
            var imgPath = System.IO.Path.Combine(GetImgPaht, imgName);
            FileInfo fileInf = new FileInfo(imgPath);

            try
            {
                if (fileInf.Exists)
                {
                    File.Delete(imgPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---UploadImageRepository----Ошибка Delete file {imgPath}");
                Console.WriteLine(ex.Message);
            }
        }
        //-----------------------------
        public byte[] Base64ImgConvertor(string PngBase64Img)
        {
            string convert = PngBase64Img.Replace("data:image/png;base64,", String.Empty);
            byte[] imgBytes = null;
            try
            {
                imgBytes = Convert.FromBase64String(convert);
            }
            catch (Exception ex)
            {
                Console.WriteLine("-----UploadImageRepository--Ошибка--Base64ImgConvertor--");
                Console.WriteLine(ex.Message);
            }

            return imgBytes;

        }



        // old vershion not resaze file img- 28.03.21
        private async void WriteOldFile(string pathPhoto, byte[] img)
        {
            Console.WriteLine($"img-Repositori-WriteOldFile --{pathPhoto} ");
            // сохраняем файл в папку Files в каталоге wwwroot
            using (var fileStream = new FileStream(pathPhoto, FileMode.Truncate))
            {
                await fileStream.WriteAsync(img);
                await fileStream.FlushAsync();
            }

        }
        // old vershion not resaze file img -28.03.21
        private void WriteNewFile(string pathPhoto, byte[] img)
        {
            Console.WriteLine($"img-Repositori writenewFile --{pathPhoto} ");
            // сохраняем файл в папку Files в каталоге wwwroot
            using (FileStream f = File.Create(pathPhoto))
            {

                f.Write(img);
                f.Flush();
            }


        }

        //   new vershion -resaze file img -28.03.21 Resaze file lib-- MagickImage
        private void SaveAndResazeImgFile(string pathPhoto, IFormFile filePhoto)
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
        //---------------------------------
        // for --test---------------
        private void SaveBase64Img(string imgPath, string imgBase64String)
        {
            using (StreamWriter sw = File.CreateText(imgPath))
            {
                sw.Write(imgBase64String);
                sw.Flush();
            }
        }
        //-------------------------------------

    }
}