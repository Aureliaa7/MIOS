using System.ComponentModel;

namespace MusicalInstrumentsShop.DataAccess.Entities
{
    public enum OrderStatus
    {
        [Description("In Progress")]
        InProgress,
        Completed,
        Canceled
    }
}