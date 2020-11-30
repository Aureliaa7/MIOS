using System.ComponentModel.DataAnnotations;

namespace MusicalInstrumentsShop.DataAccess.Entities
{
    public enum OrderStatus
    {
        [Display(Name = "In Progress")]
        InProgress,
        Completed,
        Canceled
    }
}