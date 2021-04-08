using DataAnalysisSystem.DTO.AnalysisParametersDTO.ParametersDetails;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DetailsRegressionMethodParametersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DetailsRegressionParametersViewModel parameters)
        {
            return View("_DetailsRegressionMethodParametersView", parameters);
        }
    }
}
