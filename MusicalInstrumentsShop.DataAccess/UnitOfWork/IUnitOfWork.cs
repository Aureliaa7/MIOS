﻿using MusicalInstrumentsShop.DataAccess.Entities;
using MusicalInstrumentsShop.DataAccess.Repositories.Interfaces;

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
    }
}
