using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StategyDesignPattern.FileObjectSerializer;

namespace DataAnalysisSystem.ServicesInterfaces.DesignPatterns.ChainOfResponsibility.RegexComparator
{
    public class RegexDecisionDTO
    {
        public string StringFileContent { get; set; }
        public ISerializerStrategy RegexMatchesSerializer { get; set; }
    }
}
