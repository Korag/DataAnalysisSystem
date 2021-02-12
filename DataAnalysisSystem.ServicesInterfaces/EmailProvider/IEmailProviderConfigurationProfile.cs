namespace DataAnalysisSystem.ServicesInterfaces.EmailProvider
{
    public interface IEmailProviderConfigurationProfile
    {
        string SmtpUsername { get; set; }
        string SmtpPassword { get; set; }

        string SmtpServer { get; }
        int SmtpPort { get; }
        
        string SenderName { get; set; }
    }
}
