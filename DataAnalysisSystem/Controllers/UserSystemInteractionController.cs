using DataAnalysisSystem.DTO.AdditionalFunctionalities;
using DataAnalysisSystem.DTO.UserSystemInteractionDTO;
using DataAnalysisSystem.ServicesInterfaces;
using DataAnalysisSystem.ServicesInterfaces.EmailProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAnalysisSystem.Controllers
{
    public class UserSystemInteractionController : Controller
    {
        private const string CONTACT_ACTION_NAME = "ContactWithAdministration";
        private const string USERLOGIN_ACTION_NAME = "UserLogin";
        private const string EMAIL_ATTACHMENTS_FOLDER_NAME = "resources/EmailAttachments";

        private readonly IEmailProvider _emailProvider;
        private readonly IEmailProviderConfigurationProfile _emailProviderConfigurationProfile;
        private readonly IFileHelper _fileHelper;
        
        public UserSystemInteractionController(IEmailProvider emailProvider,
                                               IEmailProviderConfigurationProfile emailProviderConfigurationProfile,
                                               IFileHelper fileHelper)
        {
            this._emailProvider = emailProvider;
            this._emailProviderConfigurationProfile = emailProviderConfigurationProfile;
            this._fileHelper = fileHelper;
        }

        [Authorize]
        [HttpGet]
        public IActionResult MainAction(string notificationMessage = null)
        {
            ViewData["Message"] = notificationMessage;

            return View();
        }

        //Empty action
        [AllowAnonymous]
        [HttpGet]
        public IActionResult AboutTheProject()
        {
            return View();
        }

        [AllowAnonymous]
        [DisableRequestSizeLimit]
        [HttpGet]
        public IActionResult ContactWithAdministration(string notificationMessage = null)
        {
            ViewData["Message"] = notificationMessage;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ContactWithAdministration(ContactWithAdministrationViewModel contactViewModel)
        {
            string notificationMessage = "Thank you for your message. Your message has been forwarded to the application administration.";
            string actionName = "";

            List<string> attachmentsPaths = _fileHelper.SaveFilesOnHardDrive(contactViewModel.Attachments, EMAIL_ATTACHMENTS_FOLDER_NAME).ToList();

            EmailMessageContentViewModel emailMessage = new EmailMessageContentViewModel(_emailProviderConfigurationProfile.SmtpUsername, 
                                                                                         _emailProviderConfigurationProfile.SenderName,
                                                                                         contactViewModel.SenderEmail,
                                                                                         contactViewModel.Topic, 
                                                                                         contactViewModel.EmailMessageContent,
                                                                                         attachmentsPaths);

            var emailSenderTask = Task.Run(() => _emailProvider.SendEmailMessageAsync(emailMessage))
                                                               .ContinueWith(result => { _fileHelper.RemoveFilesFromHardDrive(attachmentsPaths); });
           
            if (this.User.Identity.IsAuthenticated)
                actionName = CONTACT_ACTION_NAME;
            else
                actionName = USERLOGIN_ACTION_NAME;

            return RedirectToAction(actionName, "UserSystemInteraction", new { notificationMessage = notificationMessage });
        }
    }
}
