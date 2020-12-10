using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace WebShopAPI.Model
{
    public interface IUploadFile{
   public void  Save(string imgPath,IFormFile photo );
   public void  Update(string imgPath,IFormFile photo);
   public void Delete(string imgPath);
    
}
 
 //------------------------------------
 
  public class UploadImageRepository:IUploadFile{

      private readonly string _imgDir;
      private readonly IWebHostEnvironment _hostingEnvironment;

      public UploadImageRepository( IConfiguration config,IWebHostEnvironment environment){
        _imgDir= config["ImagesDir"];
        _hostingEnvironment=environment;

      }

      public void  Save(string imgPath,IFormFile photo){
           WriteNewFile(imgPath,photo);
      }
   public  void  Update(string imgPath,IFormFile photo){
             WriteOldFile(imgPath,photo);
   }
   public void Delete(string imgPath){
        throw new Exception("NOt Implimetn Exception");
   }
     
   public  string GetImgPathNewName(string imgName){
        string rootwww=   _hostingEnvironment.WebRootPath;
        var extenion=System.IO.Path.GetExtension(imgName);
        var name=Guid.NewGuid().ToString();
        Console.WriteLine("Guid file name --"+name);
        
        string filePath=System.IO.Path.Combine(rootwww, _imgDir,name+extenion);
            Console.WriteLine("filePath --"+filePath);
           return filePath;
       }

   private async void WriteOldFile(string pathPhoto,IFormFile filePhoto){
 if (filePhoto != null)
            {
                // путь к папке Files
                
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(pathPhoto, FileMode.Truncate))
                {
                    await filePhoto.CopyToAsync(fileStream);
                }
               
            }
            else{
                  throw new Exception("IFormFile filePhoto==null Exception");

            }

    }
    private async void WriteNewFile(string pathPhoto,IFormFile filePhoto){
     //    throw new Exception("NOt Implimetn Exception");
         if (filePhoto != null)
            {
                // путь к папке Files
                
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(pathPhoto, FileMode.Create))
                {
                    await filePhoto.CopyToAsync(fileStream);
                }
               
            }
            else{
                  throw new Exception("IFormFile filePhoto==null Exception");

            }

    }
  }

}