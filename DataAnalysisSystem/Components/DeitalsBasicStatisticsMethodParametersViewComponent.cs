using DataAnalysisSystem.DTO.AnalysisParametersDTO.ParametersDetails;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DetailsBasicStatisticsMethodParametersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DetailsBasicStatisticsParametersViewModel parameters)
        {
            return View("_DetailsBasicStatisticsMethodParametersView", parameters);
        }
    }
}
