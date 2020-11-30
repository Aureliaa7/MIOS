using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.ProductFiltering
{
    public interface IProductFilterService
    {
        public Task<ProductsFilteringModel> Filter(ProductsFilteringModel data, int pageSize, int? pageNumber = 1);
    }
}