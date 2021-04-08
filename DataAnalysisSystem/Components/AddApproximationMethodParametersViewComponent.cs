using DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class AddApproximationMethodParametersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AddApproximationParametersViewModel parameters)
        {
            return View("_AddApproximationMethodParametersView", parameters);
        }
    }
}
