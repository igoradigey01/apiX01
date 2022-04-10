using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using ImageMagick;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace ShopAPI.Model
{
    //------------------------------------Magick.net croup img file----------------
    // https://github.com/dlemstra/Magick.NET/blob/main/docs/ReadingImages.md

    public struct ImagesWebp
    {
        private string _syffics;
        private int _width;
        private int _quality;


        public ImagesWebp(string syffics, int width, int quality)
        {
            _syffics = syffics;
            _width = width;
            _quality = quality;

        }
        public string Syffics { get { return _syffics; } }
        public int Width { get { return _width; } }
        public int Quality { get { return _quality; } }

        public string WebpPreffic { get { return ".webp"; } }

    }
    /// <summary>
    /// CONST FOM SAVE IMG AND SRC PICTURE 
    /// </summary>
    public struct ImagesConstFromPicture
    {
        public ImagesWebp Small { get { return new ImagesWebp("S", 200, 85); } }
        public ImagesWebp Medium { get { return new ImagesWebp("M", 640, 85); } }
        public ImagesWebp Lagre { get { return new ImagesWebp("L", 1080, 95); } }
        public string PngPreffic { get { return ".png"; } }
        public int PngQuality { get { return 75; } }
        public int PnwWidth { get { return 300; } }

    }
    public class ImageRepository
    {

        private readonly string _imgDir;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ImagesConstFromPicture _syfficsImg = new ImagesConstFromPicture();



        // dir host img wwwroot/images
        private string GetImgPaht
        {
            get
            {
                string wwwroot = _hostingEnvironment.WebRootPath;
                return System.IO.Path.Combine(wwwroot, _imgDir);
            }
        }
        // generete random guid name old vertion-------
        public string GetImgRamdomName(string Imgextenion)
        {

            // var extenion = System.IO.Path.GetExtension(imgName);
            var name = Guid.NewGuid().ToString();
            //  Console.WriteLine("Guid file name --" + name);

            string fileName = System.IO.Path.Combine(name + Imgextenion);
            //Console.WriteLine("filePath --" + filePath);
            return fileName;
        }

        public string RamdomName
        {
            get
            {
                var name = Guid.NewGuid().ToString();

                return name;
            }
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


        public ImageRepository(IWebHostEnvironment environment)
        {
            _imgDir = "images";
            _hostingEnvironment = environment;

        }

        /// <summary>
        /// OLD VERTION
        /// </summary>
        /// <param name="imgName"></param>
        /// <param name="photo"></param>
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

        /// <summary>
        /// NEW VERTION 05.02.22
        /// CREATE S M L SIZE IMG webp and png
        /// </summary>
        /// <param name="imgName"></param>
        /// <param name="fileStream"></param>
        public void Save(string imgName, Stream fileStream)
        {


            ResizeAndSave(imgName, fileStream);


        }
        //---------------------------------------Convertor Base64 to Blob img

        /// <summary>
        /// OLD VERTION
        /// </summary>
        /// <param name="imgName"></param>
        /// <param name="photo"></param>
       /* public void Update(string imgName, byte[] photo)
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
        }*/

        /// <summary>
        /// NEW VERTION 07.02.22
        /// </summary>
        /// <param name="imgName"></param>
        /// <param name="fileStream"></param>
        public void Update(string imgName, Stream fileStream)
        {
            ResizeAndSave(imgName, fileStream);
        }

        //--------------------------
        /// <summary>
        /// NEW VERTION 09.02.22
        /// </summary>
        /// <param name="imgName"></param>
        public void Delete(string imgName)
        {  
            
          

            IEnumerable<string> imgList()
            {
                yield return _syfficsImg.Lagre.Syffics + imgName + ".webp";
                yield return _syfficsImg.Medium.Syffics + imgName + ".webp"; // Can be executed
                yield return _syfficsImg.Small.Syffics + imgName + ".webp";
                yield return  imgName + _syfficsImg.PngPreffic;
            }

            foreach (var i in imgList())
            {
                
            
            var imgPath = System.IO.Path.Combine(GetImgPaht, i);
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
        }
        //-----------------------------
        public byte[] Base64ImgConvertor(string webpBase64Img)
        {
            string convert = webpBase64Img.Replace("data:image/webp;base64,", String.Empty);

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

        /// <summary>
        /// NEW VERTION 07.02.22
        /// </summary>
        /// <param name="name"></param>
        /// <param name="stream"></param>
        private void ResizeAndSave(string name, Stream stream)
        {

            using (var image = new MagickImage(stream))
            {
                var pathLWebp = System.IO.Path.Combine(GetImgPaht, _syfficsImg.Lagre.Syffics + name + _syfficsImg.Lagre.WebpPreffic);
               
                using (var img = image.Clone())
                {


                    img.Write(pathLWebp);

                    var pathMWebp = System.IO.Path.Combine(GetImgPaht, _syfficsImg.Medium.Syffics + name + _syfficsImg.Medium.WebpPreffic);
                    //image.Resize()


                    // imgM.Format = MagickFormat.WebM;
                    int heightM = image.Width / image.Height * _syfficsImg.Medium.Width;
                    img.Resize(_syfficsImg.Medium.Width, heightM);
                    img.Strip();
                    img.Quality = _syfficsImg.Medium.Quality;

                    img.Write(pathMWebp);


                    var pathSWebp = System.IO.Path.Combine(GetImgPaht, _syfficsImg.Small.Syffics + name + _syfficsImg.Small.WebpPreffic);
                    // imgS.Format = MagickFormat.WebM;
                    //image.Resize()
                    var height = image.Width / image.Height * _syfficsImg.Small.Width;
                    img.Resize(_syfficsImg.Small.Width, height);
                    img.Strip();
                    img.Quality = _syfficsImg.Medium.Quality;
                    img.Write(pathSWebp);

                }


                var pathMPng = System.IO.Path.Combine(GetImgPaht+ name + _syfficsImg.PngPreffic); //08.03.22
                // Sets the output format to png
                image.Format = MagickFormat.Png;
                int heightPng = image.Width / image.Height * _syfficsImg.PnwWidth;
                image.Resize(_syfficsImg.PnwWidth, heightPng);
                image.Strip();
                image.Quality = _syfficsImg.PngQuality;
                image.Write(pathMPng);



            }
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

        private void SaveBase64Img(string imgPath, string imgBase64String)
        {
            using (StreamWriter sw = File.CreateText(imgPath))
            {
                sw.Write(imgBase64String);
                sw.Flush();
            }
        }
        //-------------------------------------
        /// <summary>
        /// template for capasity email or ....
        /// </summary>
        /// <param name="capacity"></param>
        private void CapacityImg(string capacity)
        {
            string InputImagePath = ""; // project.Variables["InputImagePath"].Value;
            string SaveImagePath = ""; // project.Variables["SaveImagePath"].Value;
            using (MagickImage image = new MagickImage(InputImagePath))
            {
                MagickReadSettings readSettings = new MagickReadSettings
                {
                    FillColor = MagickColors.Blue, // цвет текста
                    BackgroundColor = MagickColors.Transparent, // фон текста
                    Font = "Arial", // Шрифт текста (только те, что установлены в Windows)
                    Width = 350, // Ширина текста
                    Height = 500
                }; // Высота текста
                image.Alpha(AlphaOption.Opaque);
                using (MagickImage label = new MagickImage("label:Тут какой то текст", readSettings))
                {
                    image.Composite(label, 200, 100, CompositeOperator.Over); // расположение текста на картинке 200 слева, 100 сверху
                    image.Write(SaveImagePath);
                }
            }
        }
    }
}