﻿using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.UserSystemInteractionDTO
{
    public class ContactWithAdministrationViewModel
    {
        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [Display(Name = "Email address")]
        [EmailAddress(ErrorMessage = "Field \"{0}\" does not contain a valid email address.")]
        public string SenderEmail { get; set; }

        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [Display(Name = "Topic")]
        public string Topic { get; set; }

        [Required(ErrorMessage = "Field \"{0}\" is required.")]
        [Display(Name = "Message content")]
        public string EmailMessageContent { get; set; }

        [Display(Name = "Attachments")]
        public ICollection<IFormFile> Attachments { get; set; }
    }
}

