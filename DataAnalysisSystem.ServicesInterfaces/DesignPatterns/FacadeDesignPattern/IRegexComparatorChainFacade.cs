using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.ChainOfResponsibility.RegexComparator;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StrategyDesignPattern.FileObjectSerializer;

namespace DataAnalysisSystem.ServicesInterfaces.DesignPatterns.FacadeDesignPattern
{
    public interface IRegexComparatorChainFacade
    {
        public ISerializerStrategy GetSerializerStrategyBasedOnFileType(RegexDecisionDTO decisionModel);
    }
}
