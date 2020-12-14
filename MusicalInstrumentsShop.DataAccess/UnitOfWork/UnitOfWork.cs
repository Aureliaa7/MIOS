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
            DeliveryMethodRepository = new Repository<DeliveryMethod>(context);
            WishlistRepository = new WishlistRepository(context);
            WishlistProductRepository = new WishlistProductRepository(context);
            OrderDetailsRepository = new OrderDetailsRepository(context);
            OrderProductRepository = new OrderProductRepository(context);
            PaymentMethodRepository = new Repository<PaymentMethod>(context);
            CartRepository = new CartRepository(context);
            CartProductRepository = new CartProductRepository(context);
        }

        public IPhotoProductRepository PhotoProductRepository { get; private set; }

        public IProductRepository ProductRepository { get; private set; }

        public ISpecificationRepository SpecificationRepository { get; private set; }

        public IStockRepository StockRepository { get; private set; }

        public IRepository<Category> CategoryRepository { get; private set; }

        public IRepository<Photo> PhotoRepository { get; private set; }

        public IRepository<Supplier> SupplierRepository { get; private set; }

        public IRepository<DeliveryMethod> DeliveryMethodRepository { get; private set; }

        public IWishlistRepository WishlistRepository { get; private set; }

        public IWishlistProductRepository WishlistProductRepository { get; private set; }

        public IOrderDetailsRepository OrderDetailsRepository { get; private set; }

        public IOrderProductRepository OrderProductRepository { get; private set; }

        public IRepository<PaymentMethod> PaymentMethodRepository { get; private set; }
        
        public ICartRepository CartRepository { get; private set; }

        public ICartProductRepository CartProductRepository { get; private set; }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
