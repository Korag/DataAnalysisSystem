using DataAnalysisSystem.DTO.AnalysisDTO;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class AddHistogramMethodParametersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PerformNewAnalysisViewModel model)
        {
            return View("_AddHistogramMethodParametersView", model);
        }
    }
}
