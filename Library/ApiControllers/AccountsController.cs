using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Library.ApiModels;
using Library.Models;

namespace Library.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        SignInManager<LibraryUser> _signInManager;

        public AccountsController(SignInManager<LibraryUser> signInManager)
        {
            _signInManager = signInManager;
        }


        [HttpPut]
        [Route("/api/Login")]
        public async Task<ActionResult<Boolean>> Login([FromBody] Login @login)
        {
            var result = await _signInManager.PasswordSignInAsync(@login.UserName, @login.Password, false, lockoutOnFailure: false);

            return result.Succeeded;
        }

    }


}
