using System.Collections.Generic;

namespace MusicalInstrumentsShop.BusinessLogic.HelperEntities
{
    public class LoginResult
    {
        public List<string> ErrorMessages { get; set; }
        public string UserRole { get; set; }
    }
}
