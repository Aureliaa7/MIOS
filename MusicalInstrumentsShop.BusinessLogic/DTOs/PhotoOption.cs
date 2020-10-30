using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MusicalInstrumentsShop.BusinessLogic.DTOs
{
    public enum PhotoOption
    {
        [Display(Name = "Delete current photos")]
        DeleteCurrentPhotos,
        [Display(Name = "Append new photos")]
        AppendPhotos,
        [Display(Name = "Keep current photos")]
        KeepCurrentPhotos
    }
}
