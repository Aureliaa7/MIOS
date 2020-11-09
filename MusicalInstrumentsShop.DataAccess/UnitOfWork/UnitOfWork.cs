using MusicalInstrumentsShop.DataAccess.Data;
using MusicalInstrumentsShop.DataAccess.Entities;
using MusicalInstrumentsShop.DataAccess.Repositories;
using MusicalInstrumentsShop.DataAccess.Repositories.Interfaces;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;
        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;

            PhotoProductRepository = new PhotoProductRepository(context);
            ProductRepository = new ProductRepository(context);
            SpecificationRepository = new SpecificationRepository(context);
            StockRepository = new StockRepository(context);
            CategoryRepository = new Repository<Category>(context);
            PhotoRepository = new Repository<Photo>(context);
            SupplierRepository = new Repository<Supplier>(context);
        }

        public IPhotoProductRepository PhotoProductRepository { get; private set; }

        public IProductRepository ProductRepository { get; private set; }

        public ISpecificationRepository SpecificationRepository { get; private set; }

        public IStockRepository StockRepository { get; private set; }

        public IRepository<Category> CategoryRepository { get; private set; }

        public IRepository<Photo> PhotoRepository { get; private set; }

        public IRepository<Supplier> SupplierRepository { get; private set; }


        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
