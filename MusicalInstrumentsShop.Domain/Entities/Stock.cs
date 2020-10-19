namespace MusicalInstrumentsShop.Domain.Entities
{
    public class Stock
    {
        public long Id { get; set; }
        public int NumberOfProducts { get; set; }
        public Supplier Provider { get; set; }
    }
}