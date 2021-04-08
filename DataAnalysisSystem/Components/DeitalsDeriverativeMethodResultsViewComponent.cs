using DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DetailsDeriverativeMethodResultsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DetailsDeriverativeResultViewModel result)
        {
            return View("_DetailsDeriverativeMethodResultsView", result);
        }
    }
}
