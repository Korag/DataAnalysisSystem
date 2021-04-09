using DataAnalysisSystem.DTO.AnalysisDTO;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DetailsHistogramMethodResultsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AnalysisDetailsViewModel model)
        {
            return View("_DetailsHistogramMethodResultsView", model);
        }
    }
}
