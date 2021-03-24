using DataAnalysisSystem.Services.DesignPatterns.ChainOfResponsibility.RegexComparator;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.ChainOfResponsibility.RegexComparator;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.FacadeDesignPattern;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.StrategyDesignPattern.FileObjectSerializer;

namespace DataAnalysisSystem.Services.DesignPatterns.FacadeDesignPattern
{
    public class RegexComparatorChainFacade : IRegexComparatorChainFacade
    {
        private RegexComparatorAbstract _comparatorCsv { get; set; }
        private RegexComparatorAbstract _comparatorJson { get; set; }
        private RegexComparatorAbstract _comparatorXml { get; set; }
        private RegexComparatorAbstract _comparatorXlsx { get; set; }

        public RegexComparatorChainFacade()
        {
            InitializeChainOfResposonsibilityElemets();
            SetNextComparators();
        }

        private void InitializeChainOfResposonsibilityElemets()
        {
            _comparatorCsv = new RegexComparatorCsv();
            _comparatorJson = new RegexComparatorJson();
            _comparatorXml = new RegexComparatorXml();
            _comparatorXlsx = new RegexComparatorXlsx();
        }

        private void SetNextComparators()
        {
            _comparatorCsv.NextComparator = _comparatorJson;
            _comparatorJson.NextComparator = _comparatorXml;
            _comparatorXml.NextComparator = _comparatorXlsx;
        }

        public ISerializerStrategy GetSerializerStrategyBasedOnFileType(RegexDecisionDTO decisionModel)
        {
            return _comparatorCsv.StartSearchingForMatch(decisionModel);
        }
    }
}
