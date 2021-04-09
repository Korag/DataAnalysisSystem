using DataAnalysisSystem.DTO.AnalysisDTO;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DetailsRegressionMethodParametersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AnalysisDetailsViewModel modal)
        {
            return View("_DetailsRegressionMethodParametersView", modal);
        }
    }
}
