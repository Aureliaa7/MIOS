using Microsoft.AspNetCore.Http;
using MusicalInstrumentsShop.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services.Interfaces
{
    public interface IImageService
    {
        IEnumerable<Photo> SaveFiles(IEnumerable<IFormFile> formFiles);
        Task DeleteFiles(IEnumerable<string> fileNames);
    }
}
