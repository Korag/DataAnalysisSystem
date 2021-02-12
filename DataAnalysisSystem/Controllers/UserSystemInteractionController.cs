﻿using DataAnalysisSystem.DTO.AdditionalFunctionalities;
using DataAnalysisSystem.DTO.UserSystemInteractionDTO;
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
        public const string MAINACTION_ACTION_NAME = "MainAction";
        public const string USERLOGIN_ACTION_NAME = "UserLogin";

        private readonly IEmailProvider _emailProvider;
        private readonly IEmailProviderConfigurationProfile _emailProviderConfigurationProfile;
        private readonly IEmailAttachmentsHandler _emailAttachmentsHandler;
        
        public UserSystemInteractionController(IEmailProvider emailProvider,
                                               IEmailProviderConfigurationProfile emailProviderConfigurationProfile,
                                               IEmailAttachmentsHandler emailAttachmentsHandler){

            this._emailProvider = emailProvider;
            this._emailProviderConfigurationProfile = emailProviderConfigurationProfile;
            this._emailAttachmentsHandler = emailAttachmentsHandler;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult MainAction(string notificationMessage = null)
        {
            ViewData["Message"] = notificationMessage;

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AboutTheProject()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ContactWithAdministration()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ContactWithAdministration(ContactWithAdministrationViewModel contactViewModel)
        {
            string notificationMessage = "Thank you for your message. Your message has been forwarded to the application administration.";
            string actionName = "";

            List<string> attachmentsFileNames = _emailAttachmentsHandler.SaveAttachmentsOnHardDrive(contactViewModel.Attachments).ToList();

            EmailMessageContentViewModel emailMessage = new EmailMessageContentViewModel(_emailProviderConfigurationProfile.SmtpUsername, 
                                                                                         _emailProviderConfigurationProfile.SenderName, 
                                                                                         contactViewModel.SenderEmail, 
                                                                                         contactViewModel.Topic, 
                                                                                         contactViewModel.EmailMessageContent,
                                                                                         attachmentsFileNames);

            await _emailProvider.SendEmailMessageAsync(emailMessage);

            _emailAttachmentsHandler.RemoveAttachmentsFromHardDrive(attachmentsFileNames);

            if (this.User.Identity.IsAuthenticated)
                actionName = MAINACTION_ACTION_NAME;
            else
                actionName = USERLOGIN_ACTION_NAME;

            return RedirectToAction(actionName, "UserSystemInteraction", new { notificationMessage = notificationMessage });
        }
    }
}
