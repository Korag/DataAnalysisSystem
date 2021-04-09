using DataAnalysisSystem.DTO.AnalysisDTO;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DetailsApproximationMethodResultsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AnalysisDetailsViewModel modal)
        {
            return View("_DetailsApproximationMethodResultsView", modal);
        }
    }
}
