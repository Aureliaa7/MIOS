using System;

namespace MusicalInstrumentsShop.DataAccess.Entities
{
    public class Specification
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public Product Product { get; set; }
    }
}
