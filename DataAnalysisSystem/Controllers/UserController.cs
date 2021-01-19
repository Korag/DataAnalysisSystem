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

        public UserController(
                                 UserManager<IdentityProviderUser> userManager,
                                 SignInManager<IdentityProviderUser> signInManager)
        {
            
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UserLogout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("UserLogin", "User");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> UserLogin(string returnUrl = null, string notificationMessage = null)
        {
            ViewData["Message"] = notificationMessage;

            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("MainAction", "User");
            }

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
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
                        await SendEmailConfirmationMessageToUser(user);

                        return View();
                    }
                }

                var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("MainAction", "UserSystemInteraction");
                }
                else
                {
                    ModelState.AddModelError("Overall", "Wrong email or password.");

                    return View(loginViewModel);
                }
            }

            return View(loginViewModel);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> UserRegister(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword(string notificationMessage = null)
        {
            ViewData["notificationMessage"] = notificationMessage;

            return View();
        }

        [AllowAnonymous] 
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel emailModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(emailModel.Email);

                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    await SendEmailConfirmationMessageToUser(user);

                    return RedirectToAction("ForgotPassword", "UserSystemInteraction", new { notificationMessage = "To reset your password you must first confirm the email address that is associated with the account you wish to regain access to." });
                }
                await SendEmailToUser(user, "resetPassword");

                return RedirectToAction("UserLogin", "User", new { notificationMessage = "We have sent a message with further instructions to the email address associated with the account you wish to regain access to.We have sent a message with further instructions to the email address associated with the account you wish to regain access to." });
                }

            return View(emailModel);
        }

 

        public async Task<IActionResult> SendEmailConfirmationMessageToUser(IdentityProviderUser user)
        {
            //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);

            //var emailToSend = _emailSender.GenerateEmailMessage(model.Email, user.FirstName + " " + user.LastName, "emailConfirmation", callbackUrl);
            //await _emailSender.SendEmailAsync(emailToSend);
 
            return Ok();
        }

        public async Task<IActionResult> SendEmailToUser(IdentityProviderUser user, string emailTemplateName)
        {
            //var emailMessage = _emailSender.GenerateEmailMessage(emailModel.Email, "", emailTemplateName);
            //await _emailSender.SendEmailAsync(emailMessage);

            return Ok();
        }
    }
}
