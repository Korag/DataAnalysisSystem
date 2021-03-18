using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.DatasetDTO
{
    public class EditDatasetColumnTypeDoubleViewModel
    {
        public EditDatasetColumnTypeDoubleViewModel()
        {

        }

        public EditDatasetColumnTypeDoubleViewModel(string attributeName, int positionInDataset)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;

            this.AttributeValue = new List<double>();
            this.ColumnToDelete = false;
        }

        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }

        public IList<double> AttributeValue { get; set; }

        public bool ColumnToDelete { get; set; }
    }
}
