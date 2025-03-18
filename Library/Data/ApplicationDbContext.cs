using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Library.Models;

namespace Library.Data
{
    public class ApplicationDbContext : IdentityDbContext<LibraryUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Library.Models.Author> Authors { get; set; } = default!;
        public DbSet<Library.Models.Book> Books { get; set; } = default!;
        public DbSet<Library.Models.Language> Languages { get; set; } = default!;
        public DbSet<Library.Models.Parameter> Parameters { get; set; } = default!;
    }
}
