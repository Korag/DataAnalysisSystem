using DataAnalysisSystem.DTO.AnalysisDTO;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class AddDeriverativeMethodParametersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PerformNewAnalysisViewModel model)
        {
            return View("_AddDeriverativeMethodParametersView", model);
        }
    }
}
