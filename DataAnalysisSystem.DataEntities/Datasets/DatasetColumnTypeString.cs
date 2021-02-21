using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class DatasetColumnTypeString
    {
        public DatasetColumnTypeString()
        {

        }

        public DatasetColumnTypeString(string attributeName, int positionInDataset)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;

            this.AttributeValue = new List<string>();
        }

        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }

        public IList<string> AttributeValue { get; set; }
    }
}
