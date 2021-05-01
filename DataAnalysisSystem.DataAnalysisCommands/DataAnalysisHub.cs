using DataAnalysisSystem.DataAnalysisCommands.Abstract;
using DataAnalysisSystem.DataEntities;
using System.Collections.Generic;
using System.Reflection;

namespace DataAnalysisSystem.DataAnalysisCommands
{
    public class DataAnalysisHub : IDataAnalysisHub
    {
        public DataAnalysisHub()
        {

        }

        public AnalysisParameters SelectAnalysisParameters(string[] analysisMethodsName, AnalysisParameters parameters)
        {
            AnalysisParameters normalizedParameters = new AnalysisParameters();

            foreach (var methodName in analysisMethodsName)
            {
                switch (methodName)
                {
                    case "approximationMethod":
                        normalizedParameters.ApproximationParameters = parameters.ApproximationParameters;
                        break;

                    case "basicStatisticsMethod":
                        normalizedParameters.BasicStatisticsParameters = parameters.BasicStatisticsParameters;
                        break;

                    case "deriverativeMethod":
                        normalizedParameters.DeriverativeParameters = parameters.DeriverativeParameters;
                        break;

                    case "histogramMethod":
                        normalizedParameters.HistogramParameters = parameters.HistogramParameters;
                        break;

                    case "kMeansClusteringMethod":
                        normalizedParameters.KMeansClusteringParameters = parameters.KMeansClusteringParameters;
                        break;

                    case "regressionMethod":
                        normalizedParameters.RegressionParameters = parameters.RegressionParameters;
                        break;
                }
            }
            return normalizedParameters;
        }

        public List<AAnalysisCommand> SelectCommandsToPerform(string[] analysisMethodsName, IDataAnalysisService analysisService)
        {
            List<AAnalysisCommand> commandsToPerform = new List<AAnalysisCommand>();

            foreach (var methodName in analysisMethodsName)
            {
                switch (methodName)
                {
                    case "approximationMethod":
                        commandsToPerform.Add(new ApproximationCommand(analysisService));
                        break;

                    case "basicStatisticsMethod":
                        commandsToPerform.Add(new BasicStatisticsCommand(analysisService));
                        break;

                    case "deriverativeMethod":
                        commandsToPerform.Add(new DeriverativeCommand(analysisService));
                        break;

                    case "histogramMethod":
                        commandsToPerform.Add(new HistogramCommand(analysisService));
                        break;

                    case "kMeansClusteringMethod":
                        commandsToPerform.Add(new KMeansClusteringCommand(analysisService));
                        break;

                    case "regressionMethod":
                        commandsToPerform.Add(new RegressionCommand(analysisService));
                        break;
                }
            }
            return commandsToPerform;
        }

        public void ExecuteCommandsToPerformAnalysis(List<AAnalysisCommand> commandsToPerform)
        {
            foreach (var command in commandsToPerform)
            {
                command.RunAnalysis();
            }
        }

        public AnalysisResults GetAnalysisResultsFromExecutedCommands(List<AAnalysisCommand> commandsToPerform)
        {
            AnalysisResults result = new AnalysisResults();

            foreach (var command in commandsToPerform)
            {
                AnalysisResults partResult = command.GetResults();

                foreach (var property in partResult.GetType().GetProperties())
                {
                    if (property.GetValue(partResult) != null)
                    {
                        PropertyInfo piInstance = partResult.GetType().GetProperty(property.Name);
                        piInstance.SetValue(result, property.GetValue(partResult));
                        
                        break;
                    }
                }    
            }

            return result;
        }  
    }
}
