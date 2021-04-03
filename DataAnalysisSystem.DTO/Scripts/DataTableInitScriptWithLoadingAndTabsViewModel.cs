using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.Scripts
{
    public class DataTableInitScriptWithLoadingAndTabsViewModel
    {
        public string DataTableId { get; set; }
        public string LoadingIndicatorId { get; set; }
        public IList<string> TabsContentDivsId { get; set; }
    }
}
