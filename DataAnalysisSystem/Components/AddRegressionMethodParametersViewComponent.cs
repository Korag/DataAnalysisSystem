using DataAnalysisSystem.DTO.AnalysisDTO;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class AddRegressionMethodParametersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PerformNewAnalysisViewModel model)
        {
            return View("_AddRegressionMethodParametersView", model);
        }
    }
}
