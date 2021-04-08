using DataAnalysisSystem.DataAnalysisMethods;
using DataAnalysisSystem.DataEntities;

namespace DataAnalysisSystem.AkkaNet.MessagesViewModels
{
    public class CommandExecutionRequestViewModel
    {
        public IAnalysisMethod AnalysisMethod { get; set; }
        public DatasetContent DatasetContent { get; set; }
        public AnalysisParameters Parameters { get; set; }
    }
}
