using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAnalysisSystem.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Controllers
{
    public class UserSystemInteractionController : Controller
    {
        public const string MAINACTION_ACTION_NAME = "MainAction";
        public const string CONTACT_ACTION_NAME = "Contact";

        public UserSystemInteractionController()
        {

        }

        [Authorize]
        [HttpGet]
        public IActionResult MainAction()
        {
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
        public IActionResult ContactWithAdministration(string notificationMessage)
        {
            ViewData["notificationMessage"] = notificationMessage;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ContactWithAdministration(ContactWithAdministrationViewModel contactViewModel)
        {
            string notificationMessage = "Thank you for your message. Your message has been forwarded to the application administration.";
            string actionName = "";

            //Send email with attachments

            if (this.User.Identity.IsAuthenticated)
                actionName = MAINACTION_ACTION_NAME;
            else
                actionName = CONTACT_ACTION_NAME;

            return RedirectToAction(actionName, "UserSystemInteraction", new { notificationMessage = notificationMessage });
        }
    }
}
