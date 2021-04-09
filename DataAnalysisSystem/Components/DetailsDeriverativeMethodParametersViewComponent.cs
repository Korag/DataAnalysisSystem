using DataAnalysisSystem.DTO.AnalysisDTO;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DetailsDeriverativeMethodParametersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AnalysisDetailsViewModel model)
        {
            return View("_DetailsDeriverativeMethodParametersView", model);
        }
    }
}
