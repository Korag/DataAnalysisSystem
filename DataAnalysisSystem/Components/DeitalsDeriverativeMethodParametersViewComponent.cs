using DataAnalysisSystem.DTO.AnalysisParametersDTO.ParametersDetails;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DetailsDeriverativeMethodParametersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DetailsDeriverativeParametersViewModel parameters)
        {
            return View("_DetailsDeriverativeMethodParametersView", parameters);
        }
    }
}
