using Microsoft.Extensions.Caching.Memory;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.Caching
{
    public interface IProductIndexService
    {
        public Task<IEnumerable<ProductDto>> OrderByCriteria(string searchString, string orderCriteria, IMemoryCache memoryCache);
    }
}
