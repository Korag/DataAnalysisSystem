using DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class AddHistogramMethodParametersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AddHistogramParametersViewModel parameters)
        {
            return View("_AddHistogramMethodParametersView", parameters);
        }
    }
}
