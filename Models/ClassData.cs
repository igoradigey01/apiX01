using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ShopAPI.Model
{
    // class передачи сообщений Validation между repository adn controllers
    public class FlagValid
    {
        public bool Flag { get; set; }
        public string Message { get; set; }
        public object Item { get; set; }
    }

   

    // не реализован  -temp --Create, Read, Update & Delete 
    public interface ICRUD
    {
        public object[] Get(int id);
        public object Item(int id);
        public bool Create(object item);
        public bool Update(object item);
        public bool Delete(object item);


    }


    /*
        public class ModelSerialize : IValidatableObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int IdKatalog { get; set; }

            public float Price { get; set; }
            public float Markup { get; set; }
            public string Description { get; set; }
            [Required(ErrorMessage = "Please select a file.")]

            // [MaxFileSize(5* 1024 * 1024)]
            // [AllowedExtensions(new string[] { ".jpg", ".png" })]
            public IFormFile Photo { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                var photo = ((ModelSerialize)validationContext.ObjectInstance).Photo;
                var extension = System.IO.Path.GetExtension(photo.FileName);
                var size = photo.Length;

                if (!extension.ToLower().Equals(".jpg"))
                    yield return new ValidationResult("File extension is not valid.");

                if (size > (5 * 1024 * 1024))
                    yield return new ValidationResult("File size is bigger than 5MB.");
            }


        }
        */

    public class FileToUpload
    {
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string FileType { get; set; }
        public long LastModifiedTime { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string FileAsBase64 { get; set; }
        public byte[] FileAsByteArray { get; set; }
    }


}

