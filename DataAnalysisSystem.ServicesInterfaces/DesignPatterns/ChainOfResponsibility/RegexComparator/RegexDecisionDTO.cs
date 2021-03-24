using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StrategyDesignPattern.FileObjectSerializer;

namespace DataAnalysisSystem.ServicesInterfaces.DesignPatterns.ChainOfResponsibility.RegexComparator
{
    public class RegexDecisionDTO
    {
        public string FileExtension { get; set; }
        public string MimeType { get; set; }

        public ISerializerStrategy RegexMatchesSerializer { get; set; }
    }
}
