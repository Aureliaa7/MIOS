﻿using MusicalInstrumentsShop.DataAccess.Entities;
using MusicalInstrumentsShop.DataAccess.Repositories.Interfaces;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        IPhotoProductRepository PhotoProductRepository { get; }
        IProductRepository ProductRepository { get; }
        ISpecificationRepository SpecificationRepository { get; }
        IStockRepository StockRepository { get; }
        IRepository<Category> CategoryRepository { get; }
        IRepository<Photo> PhotoRepository { get; }
        IRepository<Supplier> SupplierRepository { get; }
        IRepository<DeliveryMethod> DeliveryMethodRepository { get; }
        IWishlistRepository WishlistRepository { get; }
        IWishlistProductRepository WishlistProductRepository { get; }
        IOrderDetailsRepository OrderDetailsRepository { get; }
        IOrderProductRepository OrderProductRepository { get; }
        IRepository<PaymentMethod> PaymentMethodRepository { get; }
        ICartRepository CartRepository { get; }
        ICartProductRepository CartProductRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
