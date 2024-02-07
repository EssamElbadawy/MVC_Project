﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Demo.PL.Helpers
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files", folderName);

            var fileName = $"{Guid.NewGuid()}{file.FileName}";

            var filePath = Path.Combine(folderPath, fileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;
        }

        public static async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files", folderName);

            var fileName = $"{Guid.NewGuid()}{file.FileName}";

            var filePath = Path.Combine(folderPath, fileName);

           await using var fileStream = new FileStream(filePath, FileMode.Create);

           await file.CopyToAsync(fileStream);

            return fileName;
        }

        public static void DeleteFile(string fileName, string folderName)
        {
            if (fileName is null || folderName is null) return;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files", folderName, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);


        }


    }
}
