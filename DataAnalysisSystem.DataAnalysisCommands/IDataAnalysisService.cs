using DataAnalysisSystem.DataAnalysisMethods;
using DataAnalysisSystem.DataEntities;
using System.Collections.Generic;

namespace DataAnalysisSystem.DataAnalysisCommands
{
    public interface IDataAnalysisService
    {
        void SetAnalysisType(IAnalysisMethod method);
        void RunAnalysis();
        List<AnalysisResults> GetResultsOfAllAnalyses();
    }
}