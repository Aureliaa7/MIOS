﻿using System;

namespace MusicalInstrumentsShop.DataAccess.Entities
{
    public class PhotoProduct
    {
        public Guid Id { get; set; }
        public Photo Photo { get; set; }
        public Product Product { get; set; }
    }
}
