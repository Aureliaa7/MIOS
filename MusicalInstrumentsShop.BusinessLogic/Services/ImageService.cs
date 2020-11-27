using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
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

        public async Task DeleteFiles(IEnumerable<string> fileNames)
        {
            if (fileNames != null)
            {
                foreach (var fileName in fileNames)
                {
                    string imageToBeDeleted = Path.Combine(hostEnvironment.WebRootPath, "images\\products\\", fileName);
                    if (File.Exists(imageToBeDeleted))
                    {
                        await FileIsReady(imageToBeDeleted).ContinueWith(t => File.Delete(imageToBeDeleted));
                    }
                }
            }
        }

        public async Task FileIsReady(string fileName)
        {
            await Task.Run(() =>
            {
                string path = Path.Combine(hostEnvironment.WebRootPath, "images\\products\\", fileName);
                if (!File.Exists(path))
                {
                    throw new IOException("File does not exist!");
                }
                bool isReady = false;
                while (!isReady)
                {
                    try
                    {
                        using (FileStream inputStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
                            isReady = inputStream.Length > 0;
                    }
                    catch (Exception e)
                    {
                        if (e.GetType() == typeof(IOException))
                        {
                            isReady = false;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            });
        }
    }
}
