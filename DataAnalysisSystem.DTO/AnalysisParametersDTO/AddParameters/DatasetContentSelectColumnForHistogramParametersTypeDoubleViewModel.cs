using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters
{
    public class DatasetContentSelectColumnForHistogramParametersTypeDoubleViewModel
    {
        public DatasetContentSelectColumnForHistogramParametersTypeDoubleViewModel()
        {

        }

        public DatasetContentSelectColumnForHistogramParametersTypeDoubleViewModel(string attributeName, int positionInDataset, List<double> attributeValue)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;
            this.AttributeValue = attributeValue;
        }

        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }
        public IList<double> AttributeValue { get; set; }

        public bool ColumnSelected { get; set; }

        [Range(typeof(int), "0", "2147483647")]
        public int Range { get; set; }
    }
}
