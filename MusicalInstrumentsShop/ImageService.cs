using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace MusicalInstrumentsShop
{
    public class ImageService : IImageService
    {
        private IWebHostEnvironment hostEnvironment;

        public ImageService(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public IEnumerable<Photo> SaveFiles(IEnumerable<IFormFile> formFiles)
        {
            var photos = new List<Photo>();
            string fileName = null;
            foreach (IFormFile form in formFiles)
            {
                string imageFolder = Path.Combine(hostEnvironment.WebRootPath, "images\\products");
                fileName = Guid.NewGuid().ToString() + "_" + form.FileName;
                string filePath = Path.Combine(imageFolder, fileName);
                form.CopyTo(new FileStream(filePath, FileMode.Create));
                photos.Add(new Photo { Name = fileName });
            }
            return photos;
        }

        public void DeleteFiles(IEnumerable<string> fileNames)
        {
            if (fileNames != null)
            {
                foreach (var fileName in fileNames)
                {
                    string imageToBeDeleted = Path.Combine(hostEnvironment.WebRootPath, "images\\products\\", fileName);
                    if (File.Exists(imageToBeDeleted))
                    {
                        File.Delete(imageToBeDeleted);
                    }
                }
            }
        }
    }
}
