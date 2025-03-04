using Library.Data;
using Library.Models;
using Library.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Controllers
{

    [Authorize (Roles = "UserAdmin")]
    public class UsersController : Controller
    {
        private ApplicationDbContext _context;

        public UsersController (ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string voornaam = "", string achternaam = "")
        {
            List<LibraryUser> users = _context.Users
                                        .Where(u => u.UserName != "?" 
                                                    && (voornaam == "" || u.FirstName.Contains(voornaam))
                                                    && (achternaam == "" || u.LastName.Contains(achternaam)))
                                        .OrderBy(u => u.UserName)
                                        .ToList();
            ViewData["Voornaam"] = voornaam;
            ViewData["Achternaam"] = achternaam;
            return View(users);
        }

        public IActionResult Block(string userName)
        {
            LibraryUser user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTime.MaxValue;
            _context.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult UnBlock(string userName)
        {
            LibraryUser user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTime.Now;
            _context.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Roles(string userName)
        {
            LibraryUser user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            UserRolesViewModel viewModel = new UserRolesViewModel
            {
                UserName = userName,
                Roles = (from userRole in _context.UserRoles
                         where userRole.UserId == user.Id
                         orderby userRole.RoleId
                         select userRole.RoleId).ToList()
            };
            ViewData["AllRoles"] = new MultiSelectList(_context.Roles.OrderBy(r => r.Name), "Id", "Name", viewModel.Roles);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Roles([Bind("UserName, Roles")] UserRolesViewModel model)
        {
            LibraryUser user = _context.Users.FirstOrDefault(u => u.UserName == model.UserName);

            // bestaande rollen verwijderen
            List<IdentityUserRole<string>> roles = _context.UserRoles.Where(ur => ur.UserId == user.Id).ToList();
            foreach (IdentityUserRole<string> role in roles)
            {
                _context.Remove(role);
            }

            // nieuwe rollen toekennen
            if (model.Roles != null)
                foreach (string roleId in model.Roles)
                    _context.UserRoles.Add(new IdentityUserRole<string> { RoleId = roleId, UserId = user.Id });

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
