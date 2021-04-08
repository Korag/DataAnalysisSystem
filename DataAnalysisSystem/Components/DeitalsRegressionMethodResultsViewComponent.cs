using DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DetailsRegressionMethodResultsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DetailsRegressionResultViewModel result)
        {
            return View("_DetailsRegressionMethodResultsView", result);
        }
    }
}
