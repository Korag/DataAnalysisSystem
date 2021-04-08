using DataAnalysisSystem.DataAnalysisCommands.Abstract;
using DataAnalysisSystem.DataAnalysisMethods;
using DataAnalysisSystem.DataEntities;
using System.Collections.Generic;

namespace DataAnalysisSystem.DataAnalysisCommands
{
    public class HistogramCommand : AAnalysisCommand
    {
        public HistogramCommand(IDataAnalysisService analysisService) : base(analysisService)
        {

        }

        public override void RunAnalysis()
        {
            _analysisService.SetAnalysisType(new HistogramMethod());
            _analysisService.RunAnalysis();
        }

        public override AnalysisResults GetResults()
        {
            _analysisService.SetAnalysisType(new HistogramMethod());
            return _analysisService.GetResults();
        }
    }
}
