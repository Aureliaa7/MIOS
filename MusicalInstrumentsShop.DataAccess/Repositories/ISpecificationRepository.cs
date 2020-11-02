﻿using MusicalInstrumentsShop.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.Repositories
{
    public interface ISpecificationRepository : IRepository<Specification>
    {
        Task<IEnumerable<Specification>> GetByProductId(string id);
    }
}
