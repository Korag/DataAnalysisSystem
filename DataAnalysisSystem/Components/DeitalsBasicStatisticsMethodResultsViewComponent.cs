using DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DetailsBasicStatisticsMethodResultsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DetailsBasicStatisticsResultViewModel result)
        {
            return View("_DetailsBasicStatisticsMethodResultsView", result);
        }
    }
}
