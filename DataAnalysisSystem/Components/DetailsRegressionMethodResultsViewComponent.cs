using DataAnalysisSystem.DTO.AnalysisDTO;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DetailsRegressionMethodResultsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AnalysisDetailsViewModel model)
        {
            return View("_DetailsRegressionMethodResultsView", model);
        }
    }
}
