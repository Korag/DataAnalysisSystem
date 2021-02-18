using DataAnalysisSystem.Services.DesignPatterns.StategyDesignPattern.FileObjectSerializer;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.ChainOfResponsibility.RegexComparator;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StategyDesignPattern.FileObjectSerializer;
using System.Collections.Generic;

namespace DataAnalysisSystem.Services.DesignPatterns.ChainOfResponsibility.RegexComparator
{
    public class RegexComparatorCsv : RegexComparatorAbstract
    {
        public List<string> FileExtension = new List<string> { ".CSV", ".TXT" };
        public List<string> MimeType = new List<string> { "text/csv", "application/csv", "application/vnd.ms-excel" };

        public RegexComparatorCsv()
        {
            this.onMatchFound += new OnMatchFound(RegexComparatorJson_onMatchFound);
        }

        public ISerializerStrategy RegexComparatorJson_onMatchFound(RegexComparatorAbstract comparator, RegexDecisionDTO regexDecision)
        {
            
            if (this.FileExtension.Contains(regexDecision.FileExtension) && this.MimeType.Contains(regexDecision.MimeType))
            {
                regexDecision.RegexMatchesSerializer = new CsvSerializerStrategy();
            }
            else
            {
                if (NextComparator != null)
                {
                    NextComparator.SearchForMatch(this, regexDecision);
                }
            }

            return regexDecision.RegexMatchesSerializer;
        }
    }
}
