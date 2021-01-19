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
        public const string USERLOGIN_ACTION_NAME = "UserLogin";

        public UserSystemInteractionController()
        {

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
        public IActionResult ContactWithAdministration(ContactWithAdministrationViewModel contactViewModel)
        {
            string notificationMessage = "Thank you for your message. Your message has been forwarded to the application administration.";
            string actionName = "";

            //Send email with attachments

            if (this.User.Identity.IsAuthenticated)
                actionName = MAINACTION_ACTION_NAME;
            else
                actionName = USERLOGIN_ACTION_NAME;

            return RedirectToAction(actionName, "UserSystemInteraction", new { notificationMessage = notificationMessage });
        }
    }
}
