namespace DataAnalysisSystem.DTO.Scripts
{
    public class DataTableInitScriptWithLoadingIndicatorViewModel
    {
        public DataTableInitScriptWithLoadingIndicatorViewModel()
        {
            this.SortedByColumnNumber = 0;
            this.SortingType = "asc";
        }

        public string DataTableId { get; set; }
        public string LoadingIndicatorId { get; set; }

        public int SortedByColumnNumber { get; set; }
        public string SortingType { get; set; }
    }
}
