using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StategyDesignPattern.FileObjectSerializer;

namespace DataAnalysisSystem.ServicesInterfaces.DesignPatterns.ChainOfResponsibility.RegexComparator
{
    public abstract class RegexComparatorAbstract
    {
        protected RegexComparatorAbstract nextComparator;

        public RegexComparatorAbstract NextComparator
        {
            get
            {
                return nextComparator;
            }
            set
            {
                nextComparator = value;
            }
        }

        public delegate ISerializerStrategy OnMatchFound(RegexComparatorAbstract comparator, RegexDecisionDTO regexDecision);
        public event OnMatchFound onMatchFound = null;

        public ISerializerStrategy SearchForMatch(RegexComparatorAbstract comparator, RegexDecisionDTO regexDecision)
        {
            if (onMatchFound != null)
            {
                onMatchFound(this, regexDecision);
            }

            return regexDecision.RegexMatchesSerializer;
        }

        public ISerializerStrategy StartSearchingForMatch(RegexDecisionDTO regexDecision)
        {
            return SearchForMatch(this, regexDecision);
        }
    }
}
