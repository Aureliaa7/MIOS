using System;

namespace MusicalInstrumentsShop.BusinessLogic.Exceptions
{
    public class ProductAlreadyExistsException : Exception
    {
        public ProductAlreadyExistsException() : base() { }
        public ProductAlreadyExistsException(string message) : base(message) { }
    }
}
