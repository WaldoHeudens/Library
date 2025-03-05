using Library.Models;
using Library.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using SQLitePCL;

namespace Library.Data
{
    public class ContextSeeder
    {
        private static ApplicationDbContext _context;
        private static UserManager<LibraryUser> _userManager;
        private static LibraryUser SystemAdmin = null;
        
        public static void Initialize(ApplicationDbContext context, UserManager<LibraryUser> userManager)
        {
            _context = context;
            _userManager = userManager;

            SeedRoles();
            SeedUsers();
            SeedParameters(SystemAdmin);
        }

        private static void SeedParameters(LibraryUser user)
        {
            if (!_context.Parameters.Any())
            {
                _context.AddRange(
                new Parameter { Name = "Version", Value = "0.1.0", Description = "Huidige versie van de parameterlijst", UserId = user.Id },
                    new Parameter { Name = "Mail.Server", Value = "ergens.GB_Web.be", Description = "Naam van de gebruikte pop-server", UserId = user.Id },
                    new Parameter { Name = "Mail.Port", Value = "25", Description = "Poort van de smtp-server", UserId = user.Id },
                    new Parameter { Name = "Mail.Account", Value = "SmtpServer", Description = "Acount-naam van de smtp-server", UserId = user.Id },
                    new Parameter { Name = "Mail.Password", Value = "xxxyyy!2315", Description = "Wachtwoord van de smtp-server", UserId = user.Id },
                    new Parameter { Name = "Mail.Security", Value = "true", Description = "Is SSL or TLS encryption used (true or false)", UserId = user.Id },
                    new Parameter { Name = "Mail.SenderEmail", Value = "administrator.GB_Web.be", Description = "E-mail van de smtp-verzender", UserId = user.Id },
                    new Parameter { Name = "Mail.SenderName", Value = "Administrator", Description = "Naam van de smtp-verzender", UserId = user.Id });
                _context.SaveChanges();
            }

            Parameter.Parameters = new Dictionary<string, Parameter>();
            foreach (Parameter parameter in _context.Parameters)
            {
                Parameter.Parameters[parameter.Name] = parameter;
            }

            MailKitEmailSender mailSender = (MailKitEmailSender)Globals.App.Services.GetService<IEmailSender>();
            Parameter.ConfigureMail(mailSender.Options);
        }

        private static void SeedRoles()
        {
            if (!_context.Roles.Any())
            {
                _context.Roles.AddRange(
                    new IdentityRole { Id = "User", Name = "User", NormalizedName = "USER" },
                    new IdentityRole { Id = "ContentAdmin", Name = "ContentAdmin", NormalizedName = "CONTENTADMIN" },
                    new IdentityRole { Id = "SysAdmin", Name = "SysAdmin", NormalizedName = "SYSADMIN" },
                    new IdentityRole { Id = "UserAdmin", Name = "UserAdmin", NormalizedName = "USERADMIN" }
                );
                _context.SaveChanges();
            }
        }

        private static void SeedUsers()
        {
            if (!_context.Users.Any())
            {
                LibraryUser dummyUser = new LibraryUser { Id = "?", UserName = "?", FirstName = "Dummy", LastName = "Library", Email = "Dummy@Library.be", LockoutEnabled = true, LockoutEnd = DateTime.MaxValue, PasswordHash = "ABCDE" };
                _context.Users.Add(dummyUser);

                LibraryUser Content = new LibraryUser { Id = "Content", UserName = "Content", FirstName = "Content", LastName = "Library", Email = "Content@Library.be", LockoutEnabled = false, EmailConfirmed = true };
                SystemAdmin = new LibraryUser { Id = "System", UserName = "System", FirstName = "System", LastName = "Library", Email = "System@Library.be", LockoutEnabled = false, EmailConfirmed = true };
                LibraryUser User = new LibraryUser { Id = "User", UserName = "User", FirstName = "User", LastName = "Library", Email = "Iemand@gmail.com", LockoutEnabled = false, EmailConfirmed = true };
                LibraryUser UserMan = new LibraryUser { Id = "UserMan", UserName = "UserMan", FirstName = "UserMan", LastName = "Library", Email = "User@Library.be", LockoutEnabled = false, EmailConfirmed = true };

                // Zorg ervoor dat de gebruikers een bruikbaar wachtwoord hebben
                _userManager.CreateAsync(Content, "Abc!12345");
                _userManager.CreateAsync(SystemAdmin, "Abc!12345");
                _userManager.CreateAsync(User, "Abc!12345");
                _userManager.CreateAsync(UserMan, "Abc!12345");

                _context.UserRoles.AddRange(
                    new IdentityUserRole<string> { UserId = "Content", RoleId = "ContentAdmin" },
                    new IdentityUserRole<string> { UserId = "System", RoleId = "SysAdmin" },
                    new IdentityUserRole<string> { UserId = "User", RoleId = "User" },
                    new IdentityUserRole<string> { UserId = "UserMan", RoleId = "UserAdmin" }
                );
                _context.SaveChanges();
            }
            else
                SystemAdmin = _context.Users.FirstOrDefault(u => u.UserName == "System");
        }
    }
}
