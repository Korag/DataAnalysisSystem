using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters
{
    public class DatasetColumnSelectColumnForParametersTypeString
    {
        public DatasetColumnSelectColumnForParametersTypeString()
        {

        }

        public DatasetColumnSelectColumnForParametersTypeString(string attributeName, int positionInDataset, List<string> attributeValue)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;
            this.AttributeValue = attributeValue;
        }

        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }
        public IList<string> AttributeValue { get; set; }

        public bool ColumnSelected { get; set; }
    }
}
