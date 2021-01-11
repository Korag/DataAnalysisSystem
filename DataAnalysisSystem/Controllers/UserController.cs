using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAnalysisSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityProviderUser> _userManager;
        private readonly SignInManager<IdentityProviderUser> _signInManager;
        private readonly RoleManager<IdentityProviderUserRole> _roleManager;

        public UserController()
                                 //UserManager<IdentityProviderUser> userManager,
                                 //SignInManager<IdentityProviderUser> signInManager,
                                 //RoleManager<IdentityProviderUserRole> roleManager)
        {
            //userManager = userManager;
            //signInManager = signInManager;
            //roleManager = roleManager;
        }

        [HttpGet]
        [Authorize]
        public IActionResult MainAction()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UserLogout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(UserLogin), "User");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> UserLogin(string returnUrl = null, string message = null)
        {
            ViewBag.message = message;

            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("MainAction", "User");
            }

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> UserLogin(UserLoginViewModel loginViewModel, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var user = _userManager.FindByEmailAsync(loginViewModel.Email).Result;

                if (user != null)
                {
                    var isEmailConfirmed = _userManager.IsEmailConfirmedAsync(user).Result;

                    if (!isEmailConfirmed)
                    {
                        ModelState.AddModelError("Email", "Your email address has not been confirmed so far. Confirm your address for logging in. The link has been sent to your mailbox.");

                        //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);

                        //var emailToSend = _emailSender.GenerateEmailMessage(model.Email, user.FirstName + " " + user.LastName, "emailConfirmation", callbackUrl);
                        //await _emailSender.SendEmailAsync(emailToSend);

                        return View();
                    }
                }

                var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(MainAction), "User");
                }
                else
                {
                    ModelState.AddModelError("Overall", "Wrong email or password.");

                    return View(loginViewModel);
                }
            }

            return View(loginViewModel);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> UserRegister(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
    }
}
