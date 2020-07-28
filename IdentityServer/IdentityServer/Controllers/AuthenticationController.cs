using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Data;
using IdentityServer.Models;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly AuthenticationDbContext _authContext;

        public AuthenticationController(AuthenticationDbContext authContext)
        {
            _authContext = authContext;
        }


        [Route("login", Name = "UrlLogin")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        [AutoValidateAntiforgeryToken]
        public IActionResult Login(AuthenticationLoginModel model)
        {
         
            if (ModelState.IsValid)
            {
                var user = _authContext.Users.SingleOrDefault(x => x.Login == model.UserName && x.Password == model.Password.Sha256());
                if(user != null)
                {
                    var isUser = new IdentityServerUser(user.ID.ToString())
                    {
                        DisplayName = $"{user.Firstname} {user.Name}"
                    };


                    HttpContext.SignInAsync(isUser);
                }
                ModelState.AddModelError("UserName", "Login / mot de passe invalide");
            }
            return View();
        }

        [Route("logout", Name ="UrlLogout")]
        public async Task<IActionResult> Logout()
        {
            var u = this.User;
            

            if (User?.Identity.IsAuthenticated == true)
            {
                await HttpContext.SignOutAsync();
                return View();
            }
            else
            {
                return RedirectToRoute("UrlLogin");
            }
        }
    }
}
