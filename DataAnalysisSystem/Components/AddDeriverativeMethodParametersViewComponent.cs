using DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class AddDeriverativeMethodParametersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AddDeriverativeParametersViewModel parameters)
        {
            return View("_AddDeriverativeMethodParametersView", parameters);
        }
    }
}
