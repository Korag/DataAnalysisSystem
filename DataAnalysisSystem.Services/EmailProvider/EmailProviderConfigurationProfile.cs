using DataAnalysisSystem.ServicesInterfaces.EmailProvider;

namespace DataAnalysisSystem.Services.EmailProvider
{
    public class EmailProviderConfigurationProfile : IEmailProviderConfigurationProfile
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SenderName { get; set; }
        public string SmtpPassword { get; set; }
    }
}
