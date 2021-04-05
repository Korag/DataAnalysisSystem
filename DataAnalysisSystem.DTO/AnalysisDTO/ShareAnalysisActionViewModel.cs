using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.AnalysisDTO
{
    public class ShareAnalysisActionViewModel
    {
        public ShareAnalysisActionViewModel()
        {
            this.SharedAnalyses = new List<SharedAnalysisByOwnerViewModel>();
            this.NotSharedAnalyses = new List<NotSharedAnalysisViewModel>();
        }

        public List<SharedAnalysisByOwnerViewModel> SharedAnalyses { get; set; }
        public List<NotSharedAnalysisViewModel> NotSharedAnalyses { get; set; }
    }
}
