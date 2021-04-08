using DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class AddBasicStatisticsMethodParametersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AddBasicStatisticsParametersViewModel parameters)
        {
            return View("_AddBasicStatisticsMethodParametersView", parameters);
        }
    }
}
