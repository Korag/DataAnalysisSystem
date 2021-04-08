using DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class AddRegressionMethodParametersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AddRegressionParametersViewModel parameters)
        {
            return View("_AddRegressionMethodParametersView", parameters);
        }
    }
}
