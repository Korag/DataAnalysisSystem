using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.DatasetDTO
{
    public class EditDatasetColumnTypeStringViewModel
    {
        public EditDatasetColumnTypeStringViewModel()
        {

        }

        public EditDatasetColumnTypeStringViewModel(string attributeName, int positionInDataset)
        {
            this.AttributeName = attributeName;
            this.PositionInDataset = positionInDataset;

            this.AttributeValue = new List<string>();
            this.ColumnToDelete = false;
        }

        public string AttributeName { get; set; }
        public int PositionInDataset { get; set; }

        public IList<string> AttributeValue { get; set; }

        public bool ColumnToDelete { get; set; }
    }
}
