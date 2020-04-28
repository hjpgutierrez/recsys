using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace WebApiRecSys
{
    public class FileUploadAPI
    {
        public IFormFile files {get; set;}

        public static string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return  Path.GetFileNameWithoutExtension(fileName)
                    + "_" 
                    + Guid.NewGuid().ToString().Substring(0, 4) 
                    + Path.GetExtension(fileName);
        }
    }
}