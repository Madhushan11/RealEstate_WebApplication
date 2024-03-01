using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EstateExplore.Data;
using EstateExplore.Models;
using System.Security.Claims;

namespace EstateExplore.Controllers
{
    public class AccountController : Controller
    {
        private readonly RealEstateContext _context;

        public AccountController(RealEstateContext context) 
        {
            _context = context;
        }

        public IActionResult Login(string returnUrl="/")
        {
            Login loginModel = new Login();
            loginModel.ReturnUrl = returnUrl;
            return View(loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login loginModel)
        {
            //RentalSystemContext userContext = new RentalSystemContext();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginModel.UserName && u.Password == loginModel.Password);
            if(user!=null)
            {
                /*var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.UserId)),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("Admin", "Code")

                };*/

                var claims = new List<Claim>();
                if (user.UserId != null)
                {
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.UserId)));
                }
                if (!string.IsNullOrEmpty(user.Username))
                {
                    claims.Add(new Claim(ClaimTypes.Name, user.Username));
                }
                if (!string.IsNullOrEmpty(user.Role))
                {
                    claims.Add(new Claim(ClaimTypes.Role, user.Role));
                }
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal,
                    new AuthenticationProperties()
                    {
                        IsPersistent = loginModel.RememberLogin
                    });
                return LocalRedirect(loginModel.ReturnUrl);
            }
            else
            {
                ViewBag.Message = "Invalid Credential";
                return View(loginModel);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/");
        }
    }
}
