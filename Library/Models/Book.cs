using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Library.Models
{
    public class Book
    {
        public int Id { get; set; }
        
        [Display (Name = "ISBN-nummer")]
        [Required]
        public string ISBN { get; set; }

        [Required]
        [Display(Name = "Titel")]
        public string Title { get; set; }

        [ForeignKey ("Authors")]

        [Display(Name = "Auteur")]
        public int AuthorId { get; set; }
 
        [Display(Name = "Auteur")]
        public Author? Author { get; set; }

        [Display(Name = "Datum uitgifte")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        public DateTime Deleted { get; set; } = DateTime.MaxValue;
    }
}
