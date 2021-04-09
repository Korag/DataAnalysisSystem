using DataAnalysisSystem.DTO.AnalysisDTO;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DetailsHistogramMethodParametersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AnalysisDetailsViewModel modal)
        {
            return View("_DetailsHistogramMethodParametersView", modal);
        }
    }
}
