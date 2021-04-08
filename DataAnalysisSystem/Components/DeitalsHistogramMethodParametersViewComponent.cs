using DataAnalysisSystem.DTO.AnalysisParametersDTO.ParametersDetails;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DetailsHistogramMethodParametersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DetailsHistogramParametersViewModel parameters)
        {
            return View("_DetailsHistogramMethodParametersView", parameters);
        }
    }
}
