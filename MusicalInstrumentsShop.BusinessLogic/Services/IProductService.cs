using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public interface IProductService
    {
        public Task AddNew(AddProductDto product, IEnumerable<Photo> photos);
        public Task<IEnumerable<ProductDto>> GetAll();
        public Task<ProductDto> GetById(string id);
        public Task<IEnumerable<string>> Delete(string id);
        public Task Update(UpdateProductDto productDto, IEnumerable<Photo> photos);
        public Task<UpdateProductDto> GetForUpdate(string id);
    }
}
