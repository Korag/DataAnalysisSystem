using DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DetailsApproximationMethodResultsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DetailsApproximationResultViewModel result)
        {
            return View("_DetailsApproximationMethodResultsView", result);
        }
    }
}
