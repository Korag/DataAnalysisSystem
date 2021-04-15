using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters
{
    public class DatasetColumnSelectColumnForParametersTypeStringViewModel
    {
        public DatasetColumnSelectColumnForParametersTypeStringViewModel()
        {

        }

        public DatasetColumnSelectColumnForParametersTypeStringViewModel(string attributeName, int positionInDataset, bool columnsSelected = false)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;
            this.ColumnSelected = columnsSelected;
        }

        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }

        public bool ColumnSelected { get; set; }
    }
}
