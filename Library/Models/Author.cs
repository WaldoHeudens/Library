using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Display (Name = "Voornaam")]
        public string FirstName { get; set; }

        [Display(Name = "Achternaam")]
        public string LastName { get; set; }

        [Display(Name = "Een beetje uitleg")]
        public string Description { get; set; }

        public DateTime Deleted { get; set; } = DateTime.MaxValue;

    }
}
