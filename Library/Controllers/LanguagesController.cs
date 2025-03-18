using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Library.Data;
using Library.Models;

namespace GB_Web.Controllers
{
    public class LanguagesController : Controller
    {
        public LanguagesController()
        {
        }

        public IActionResult ChangeLanguage(string id, string returnUrl)
        {


            // Do not implement culture yet, only language

            //string culture = Thread.CurrentThread.CurrentCulture.ToString();
            //try
            //{
            //    culture = id + culture.Substring(2, 3);
            //}
            //catch
            //{
            //    culture = id + "-BE";
            //}

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(id)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }
    }
}
