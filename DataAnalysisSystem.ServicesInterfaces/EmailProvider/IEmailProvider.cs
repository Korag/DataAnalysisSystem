using DataAnalysisSystem.DTO.AdditionalFunctionalities;
using System.Threading.Tasks;

namespace DataAnalysisSystem.ServicesInterfaces.EmailProvider
{
    public interface IEmailProvider
    {
        Task SendEmailMessageAsync(EmailMessageContentViewModel emailContent);
    }
}
