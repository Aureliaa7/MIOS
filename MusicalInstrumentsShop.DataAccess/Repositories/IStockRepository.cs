﻿using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.Repositories
{
    public interface IStockRepository : IRepository<Stock>
    {
        Task<Stock> GetWithRelatedData(Guid id);
        Task<Stock> GetByProductId(string productId);
    }
}
