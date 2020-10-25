using System;
using System.ComponentModel.DataAnnotations;

namespace MusicalInstrumentsShop.DataAccess.Entities
{
    public class Supplier
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }
    }
}