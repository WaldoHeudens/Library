using Library.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Index()
        {
            return View(_context.Users.Where(u => u.UserName != "?").OrderBy(u => u.UserName));
        }

        public IActionResult Roles()
        {

            return View();
        }
    }
}
