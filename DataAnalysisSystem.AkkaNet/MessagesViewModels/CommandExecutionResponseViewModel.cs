using DataAnalysisSystem.DataEntities;

namespace DataAnalysisSystem.AkkaNet.MessagesViewModels
{
    public class CommandExecutionResponseViewModel
    {
        public AnalysisResults AnalysisResult { get; set; }

        public CommandExecutionResponseViewModel(AnalysisResults result)
        {
            AnalysisResult = result;
        }
    }
}
