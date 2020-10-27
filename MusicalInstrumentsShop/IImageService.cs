using Microsoft.AspNetCore.Http;
using MusicalInstrumentsShop.DataAccess.Entities;
using System.Collections.Generic;

namespace MusicalInstrumentsShop
{
    public interface IImageService
    {
        IEnumerable<Photo> SaveFiles(IEnumerable<IFormFile> formFiles);
        void DeleteFiles(IEnumerable<string> fileNames);
    }
}
