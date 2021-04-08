using DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DetailsHistogramMethodResultsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DetailsHistogramResultViewModel result)
        {
            return View("_DetailsHistogramMethodResultsView", result);
        }
    }
}
