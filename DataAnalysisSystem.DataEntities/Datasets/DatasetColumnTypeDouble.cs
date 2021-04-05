using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class DatasetColumnTypeDouble : IDatasetColumnType
    {
        public DatasetColumnTypeDouble()
        {

        }

        public DatasetColumnTypeDouble(string attributeName, int positionInDataset)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;

            this.AttributeValue = new List<double>();
        }

        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }

        public IList<double> AttributeValue { get; set; }
    }
}
