namespace DataAnalysisSystem.DataEntities
{
    public interface IDatasetColumnType
    {
        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }
    }
}
