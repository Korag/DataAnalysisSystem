using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.DatasetDTO
{
    public class ShareDatasetActionViewModel
    {
        public ShareDatasetActionViewModel()
        {
            this.SharedDatasets = new List<SharedDatasetByOwnerViewModel>();
            this.NotSharedDatasets = new List<NotSharedDatasetViewModel>();
        }

        public List<SharedDatasetByOwnerViewModel> SharedDatasets { get; set; }
        public List<NotSharedDatasetViewModel> NotSharedDatasets { get; set; }
    }
}
