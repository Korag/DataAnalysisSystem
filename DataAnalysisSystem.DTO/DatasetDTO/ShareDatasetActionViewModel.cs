using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.DatasetDTO
{
    public class ShareDatasetActionViewModel
    {
        public ShareDatasetActionViewModel()
        {
            this.SharedDatasets = new List<SharedDatasetViewModel>();
            this.NotSharedDatasets = new List<NotSharedDatasetViewModel>();
        }

        public List<SharedDatasetViewModel> SharedDatasets { get; set; }
        public List<NotSharedDatasetViewModel> NotSharedDatasets { get; set; }
    }
}
