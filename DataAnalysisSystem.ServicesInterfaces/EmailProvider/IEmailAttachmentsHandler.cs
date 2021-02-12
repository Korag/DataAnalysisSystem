using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace DataAnalysisSystem.ServicesInterfaces.EmailProvider
{
    public interface IEmailAttachmentsHandler
    {
        ICollection<string> SaveAttachmentsOnHardDrive(ICollection<IFormFile> attachments);
        void RemoveAttachmentsFromHardDrive(ICollection<string> attachmentsFileNames);
    }
}
