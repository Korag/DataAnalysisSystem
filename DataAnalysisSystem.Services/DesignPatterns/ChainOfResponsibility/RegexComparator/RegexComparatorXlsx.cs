using DataAnalysisSystem.Services.DesignPatterns.StategyDesignPattern.FileObjectSerializer;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.ChainOfResponsibility.RegexComparator;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StategyDesignPattern.FileObjectSerializer;
using System.Collections.Generic;

namespace DataAnalysisSystem.Services.DesignPatterns.ChainOfResponsibility.RegexComparator
{
    public class RegexComparatorXlsx : RegexComparatorAbstract
    {
        public List<string> FileExtension = new List<string> { ".XLSX", ".XLS" };
        public List<string> MimeType = new List<string> { "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };

        public RegexComparatorXlsx()
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