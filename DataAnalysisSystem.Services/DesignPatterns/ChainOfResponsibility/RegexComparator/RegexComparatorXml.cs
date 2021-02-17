using DataAnalysisSystem.Services.DesignPatterns.StategyDesignPattern.FileObjectSerializer;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.ChainOfResponsibility.RegexComparator;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StategyDesignPattern.FileObjectSerializer;

namespace DataAnalysisSystem.Services.DesignPatterns.ChainOfResponsibility.RegexComparator
{
    public class RegexComparatorXml : RegexComparatorAbstract
    {
        public string RegexExpression = "";

        public RegexComparatorXml()
        {
            this.onMatchFound += new OnMatchFound(RegexComparatorJson_onMatchFound);
        }

        public ISerializerStrategy RegexComparatorJson_onMatchFound(RegexComparatorAbstract comparator, RegexDecisionDTO regexDecision)
        {
            // check if regex matches
            if (true)
            {
                regexDecision.RegexMatchesSerializer = new XmlSerializerStrategy();
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
