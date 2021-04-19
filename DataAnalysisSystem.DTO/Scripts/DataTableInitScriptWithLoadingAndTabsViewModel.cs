using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.Scripts
{
    public class DataTableInitScriptWithLoadingAndTabsViewModel
    {
        public DataTableInitScriptWithLoadingAndTabsViewModel()
        {
            this.SortedByColumnNumber = 0;
            this.SortingType = "asc";
        }

        public string DataTableId { get; set; }
        public string LoadingIndicatorId { get; set; }
        public IList<string> TabsContentDivsId { get; set; }

        public int SortedByColumnNumber { get; set; }
        public string SortingType { get; set; }
    }
}
