using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters
{
    public class DatasetColumnSelectColumnForParametersTypeDouble
    {
        public DatasetColumnSelectColumnForParametersTypeDouble()
        {

        }

        public DatasetColumnSelectColumnForParametersTypeDouble(string attributeName, int positionInDataset, List<double> attributeValue)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;
            this.AttributeValue = attributeValue;
        }

        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }
        public IList<double> AttributeValue { get; set; }

        public bool ColumnSelected { get; set; }
    }
}
