using DataAnalysisSystem.DTO.Dictionaries;
using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.AdditionalFunctionalities
{
    public class EmailMessageContentViewModel
    {
        public string EmailRecipientAddress { get; set; }
        public string EmailRecipientFullName { get; set; }
       
        public string EmailTopic { get; set; }
        public string HeaderOfEmailContent { get; set; }
        public string PrimaryContent { get; set; }
        public string SecondaryContent { get; set; }

        public string AdditionalURLToAction { get; set; }
        public string URLActionText { get; set; }

        public ICollection<string> AttachmentsPaths { get; set; }

        public string EmailSenderAddress { get; private set; }

        public EmailMessageContentViewModel(string emailRecipientAddress, 
                                            string emailRecipientFullName, 
                                            string emailClassifierKey, 
                                            string additionalUrlToAction = null,
                                            ICollection<string> attachmentsPaths = null){

            this.EmailRecipientAddress = emailRecipientAddress;
            this.EmailRecipientFullName = emailRecipientFullName;

            this.EmailTopic = EmailClassifierDictionary.EmailTopic[emailClassifierKey];
            this.HeaderOfEmailContent = EmailClassifierDictionary.HeaderOfEmailContent[emailClassifierKey];
            this.PrimaryContent = EmailClassifierDictionary.PrimaryContent[emailClassifierKey];
            this.SecondaryContent = EmailClassifierDictionary.SecondaryContent[emailClassifierKey];

            this.AdditionalURLToAction = additionalUrlToAction;
            this.URLActionText = EmailClassifierDictionary.URLActionText[emailClassifierKey];

            if (attachmentsPaths != null)
            {
                this.AttachmentsPaths = attachmentsPaths;
            }
            else
            {
                this.AttachmentsPaths = new List<string>();
            }
        }

        public EmailMessageContentViewModel(string emailRecipientAddress,
                                            string emailRecipientFullName,
                                            string emailSenderAddress,
                                            string emailTopic,
                                            string primaryContent,
                                            ICollection<string> attachmentsPaths = null)
        {
            this.EmailRecipientAddress = emailRecipientAddress;
            this.EmailRecipientFullName = emailRecipientFullName;
            this.EmailSenderAddress = emailSenderAddress;

            this.EmailTopic = emailTopic;
            this.PrimaryContent = primaryContent;

            if (attachmentsPaths != null)
            {
                this.AttachmentsPaths = attachmentsPaths;
            }
            else
            {
                this.AttachmentsPaths = new List<string>();
            }
        }
    }
}
