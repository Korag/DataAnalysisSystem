using AutoMapper;
using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.DTO.AdditionalFunctionalities;
using DataAnalysisSystem.DTO.UserDTO;
using DataAnalysisSystem.Extensions;
using DataAnalysisSystem.ServicesInterfaces;
using DataAnalysisSystem.ServicesInterfaces.EmailProvider;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DataAnalysisSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityProviderUser> _userManager;
        private readonly SignInManager<IdentityProviderUser> _signInManager;

        private readonly ICodeGenerator _codeGenerator;
        private readonly IEmailProvider _emailProvider;
        private readonly IMapper _autoMapper;

        public UserController(
                              UserManager<IdentityProviderUser> userManager,
                              SignInManager<IdentityProviderUser> signInManager,
                              ICodeGenerator codeGenerator,
                              IEmailProvider emailProvider,
                              IMapper autoMapper){

            this._userManager = userManager;
            this._signInManager = signInManager;

            this._codeGenerator = codeGenerator;
            this._emailProvider = emailProvider;

            this._autoMapper = autoMapper;
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
                var loggedUser = _userManager.FindByEmailAsync(loginViewModel.Email).Result;

                if (loggedUser != null)
                {
                    var isEmailConfirmed = _userManager.IsEmailConfirmedAsync(loggedUser).Result;

                    if (!isEmailConfirmed)
                    {
                        ModelState.AddModelError("Email", "Your email address has not been confirmed so far. Confirm your address for logging in. The link has been sent to your mailbox.");

                        var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(loggedUser);
                        var callbackUrl = Url.GenerateEmailConfirmationLink(loggedUser.Id.ToString(), emailConfirmationToken, Request.Scheme);

                        await SendEmailMessageToUser(loggedUser, "emailConfirmation", callbackUrl);

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

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UserRegister(UserRegisterViewModel registerViewModel, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var newUser = _autoMapper.Map<IdentityProviderUser>(registerViewModel);
                newUser.Id = _codeGenerator.GenerateNewDbEntityUniqueIdentificatorAsObjectId();
                newUser.SecurityStamp = _codeGenerator.GenerateNewUniqueCodeAsString();

                var result = await _userManager.CreateAsync(newUser, registerViewModel.Password);

                if (result.Succeeded)
                {
                    var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    var callbackUrl = Url.GenerateEmailConfirmationLink(newUser.Id.ToString(), emailConfirmationToken, Request.Scheme);

                    await SendEmailMessageToUser(newUser, "emailConfirmation", callbackUrl);

                    return RedirectToAction("UserLogin", "User", new { notificationMessage = "A message has been sent to your email inbox to confirm the email address you entered during registration." });
                }

                ModelState.AddModelError(string.Empty, "A user with the specified email address already exists in the system.");
            }

            return View(registerViewModel);
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
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgottenpasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgottenpasswordViewModel.Email);
                var callbackUrl = "";

                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    callbackUrl = Url.GenerateEmailConfirmationLink(user.Id.ToString(), emailConfirmationToken, Request.Scheme);

                    await SendEmailMessageToUser(user, "emailConfirmation", callbackUrl);

                    return RedirectToAction("ForgotPassword", "UserSystemInteraction", new { notificationMessage = "To reset your password you must first confirm the email address that is associated with the account you wish to regain access to." });
                }
 
                var passwordResetAuthorizationToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                callbackUrl = Url.GenerateResetForgottenPasswordLink(user.Id.ToString(), passwordResetAuthorizationToken, Request.Scheme);

                await SendEmailMessageToUser(user, "resetPassword", callbackUrl);

                return RedirectToAction("UserLogin", "User", new { notificationMessage = "We have sent a message with further instructions to the email address associated with the account you wish to regain access to.We have sent a message with further instructions to the email address associated with the account you wish to regain access to." });
            }

            return View(forgottenpasswordViewModel);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ChangeForgottenPassword(string userIdentificator, string authorizationToken = null)
        {
            var user = await _userManager.FindByIdAsync(userIdentificator);

            if (authorizationToken == null || user == null)
            {
                return RedirectToAction("UserLogin", "User");
            }

            ChangeForgottenPasswordViewModel resetPasswordViewModel = new ChangeForgottenPasswordViewModel(userIdentificator, authorizationToken);

            return View(resetPasswordViewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ChangeForgottenPassword(ChangeForgottenPasswordViewModel changedPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.FindByIdAsync(changedPasswordViewModel.UserIdentificator).Result;

                if (user == null)
                {
                    return RedirectToAction("UserLogin", "User");
                }

                var result = await _userManager.ResetPasswordAsync(user, changedPasswordViewModel.AuthorizationToken, changedPasswordViewModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("UserLogin", "User", new { message = "Your password has been changed." });
                }
            }

            return View(changedPasswordViewModel);
        }

        public async Task<IActionResult> SendEmailMessageToUser(IdentityProviderUser user, string emailClassifierKey, string additionalURL = "")
        {   
            EmailMessageContentViewModel emailMessage = new EmailMessageContentViewModel(user.Email, user.FirstName + " " + user.LastName, emailClassifierKey, additionalURL);
            await _emailProvider.SendEmailMessageAsync(emailMessage);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmailAddress(EmailConfirmationViewModel emailConfirmation)
        {
            string notificationMessageText = null;

            if (ModelState.IsValid)
            {
                var userWNotConfirmedEmail = _userManager.FindByIdAsync(emailConfirmation.UserIdentificator).Result;

                if (userWNotConfirmedEmail != null)
                {
                    var result = await _userManager.ConfirmEmailAsync(userWNotConfirmedEmail, emailConfirmation.AuthorizationToken);

                    if (result.Succeeded)
                    {
                        notificationMessageText = "The email address has been confirmed.";
                    }
                }

                return RedirectToAction("UserLogin", "User", new { notificationMessage = notificationMessageText });
            }

            return RedirectToAction("UserLogin", "User", new { notificationMessage = notificationMessageText });
        }
    }
}
