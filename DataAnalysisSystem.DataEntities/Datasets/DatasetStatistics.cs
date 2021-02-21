namespace DataAnalysisSystem.DataEntities
{
    public class DatasetStatistics
    {
        public DatasetStatistics()
        {

        }

        public int NumberOfColumns { get; set; }
        public int NumberOfRows { get; set; }
        public int NumberOfMissingValues { get; set; }

        public string InputFileFormat { get; set; }
        public string InputFileName { get; set; }
    }
}
