using DataAnalysisSystem.DTO.AnalysisDTO;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DetailsDeriverativeMethodResultsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AnalysisDetailsViewModel model)
        {
            return View("_DetailsDeriverativeMethodResultsView", model);
        }
    }
}
