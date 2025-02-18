using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class LibraryUser:IdentityUser
    {
        [Required]
        [Display (Name = "Voornaam")]
        public string FirstName { get; set; }

        [Display(Name = "Achternaam")]
        [Required]
        public string LastName { get; set; }
    }
}
