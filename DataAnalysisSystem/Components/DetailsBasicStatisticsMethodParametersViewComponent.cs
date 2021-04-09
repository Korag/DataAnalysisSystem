using DataAnalysisSystem.DTO.AnalysisDTO;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DetailsBasicStatisticsMethodParametersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AnalysisDetailsViewModel model)
        {
            return View("_DetailsBasicStatisticsMethodParametersView", model);
        }
    }
}
