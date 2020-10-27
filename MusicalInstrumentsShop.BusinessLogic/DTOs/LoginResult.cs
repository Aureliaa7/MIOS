using System.Collections.Generic;

namespace MusicalInstrumentsShop.BusinessLogic.DTOs
{
    public class LoginResult
    {
        public List<string> ErrorMessages { get; set; }
        public string UserRole { get; set; }

        public LoginResult()
        {
            ErrorMessages = new List<string>();
        }
    }
}
