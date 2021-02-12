using DataAnalysisSystem.ServicesInterfaces.EmailProvider;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;

namespace DataAnalysisSystem.Services.EmailProvider
{
    public class EmailAttachmentsHandler : IEmailAttachmentsHandler
    {
        private IHostingEnvironment _environment;
        private const string ATTACHMENTS_FOLDER_NAME = "EmailAttachments";

        public EmailAttachmentsHandler(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public ICollection<string> SaveAttachmentsOnHardDrive(ICollection<IFormFile> attachments)
        {
            List<string> attachmentsFileNames = new List<string>();

            foreach (var attachment in attachments)
            {
                var pathToFile = Path.Combine(_environment.WebRootPath, ATTACHMENTS_FOLDER_NAME, attachment.FileName);

                using (var fileStream = new FileStream(pathToFile, FileMode.Create))
                {
                    attachment.CopyTo(fileStream);
                    attachmentsFileNames.Add(attachment.FileName);
                }
            }

            return attachmentsFileNames;
        }

        public void RemoveAttachmentsFromHardDrive(ICollection<string> attachmentsFileNames)
        {
            if (attachmentsFileNames != null && attachmentsFileNames.Count != 0)
            {
                foreach (var attachmentFileName in attachmentsFileNames)
                {
                    var pathToFile = Path.Combine(_environment.WebRootPath, ATTACHMENTS_FOLDER_NAME, attachmentFileName);
                    File.Delete(pathToFile);
                }
            }
        }
    }
}
