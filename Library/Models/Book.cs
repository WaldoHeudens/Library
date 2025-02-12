using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string ISBN { get; set; } 
        public string Title { get; set; }

        [ForeignKey ("Authors")]
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}
