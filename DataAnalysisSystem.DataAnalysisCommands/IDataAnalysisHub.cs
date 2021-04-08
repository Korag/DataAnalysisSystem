using DataAnalysisSystem.DataAnalysisCommands.Abstract;
using DataAnalysisSystem.DataEntities;
using System.Collections.Generic;

namespace DataAnalysisSystem.DataAnalysisCommands
{
    public interface IDataAnalysisHub
    {
        public AnalysisParameters SelectAnalysisParameters(string[] analysisMethodsName, AnalysisParameters parameters);
        public List<AAnalysisCommand> SelectCommandsToPerform(string[] analysisMethodsName, IDataAnalysisService analysisService);
        public void ExecuteCommandsToPerformAnalysis(List<AAnalysisCommand> commandsToPerform);
        public AnalysisResults GetAnalysisResultsFromExecutedCommands(List<AAnalysisCommand> commandsToPerform);
    }
}
