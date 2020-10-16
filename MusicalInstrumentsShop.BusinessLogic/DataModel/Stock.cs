namespace MusicalInstrumentsShop.BusinessLogic.DataModel
{
    public class Stock
    {
        public long Id { get; set; }
        public int NumberOfProducts { get; set; }
        public Provider Provider { get; set; }
    }
}