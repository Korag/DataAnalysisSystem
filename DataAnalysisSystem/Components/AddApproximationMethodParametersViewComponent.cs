using DataAnalysisSystem.DTO.AnalysisDTO;
using DataAnalysisSystem.DTO.AnalysisParametersDTO;
using DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class AddApproximationMethodParametersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PerformNewAnalysisViewModel model)
        {
            return View("_AddApproximationMethodParametersView", model);
        }
    }
}
