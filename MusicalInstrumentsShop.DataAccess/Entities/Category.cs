using System;
using System.ComponentModel.DataAnnotations;

namespace MusicalInstrumentsShop.DataAccess.Entities
{
    public class Category
    { 
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}