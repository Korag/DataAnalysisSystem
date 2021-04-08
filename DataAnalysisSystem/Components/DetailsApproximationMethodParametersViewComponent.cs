using DataAnalysisSystem.DTO.AnalysisParametersDTO.ParametersDetails;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DetailsApproximationMethodParametersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DetailsApproximationParametersViewModel parameters)
        {
            return View("_DetailsApproximationMethodParametersView", parameters);
        }
    }
}
