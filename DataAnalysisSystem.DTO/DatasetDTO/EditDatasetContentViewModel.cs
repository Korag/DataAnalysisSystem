using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.DatasetDTO
{
    public class EditDatasetContentViewModel
    {
        public EditDatasetContentViewModel()
        {
            this.NumberColumns = new List<EditDatasetColumnTypeDoubleViewModel>();
            this.StringColumns = new List<EditDatasetColumnTypeStringViewModel>();

            this.RowsToDelete = new List<bool>();
        }

        public IList<EditDatasetColumnTypeDoubleViewModel> NumberColumns { get; set; }
        public IList<EditDatasetColumnTypeStringViewModel> StringColumns { get; set; }
        public IList<bool> RowsToDelete { get; set; }
    }
}
