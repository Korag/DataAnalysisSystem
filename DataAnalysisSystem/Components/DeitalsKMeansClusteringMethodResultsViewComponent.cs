using DataAnalysisSystem.DTO.AnalysisResultsDTO.AnalysisResultsDetails;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DetailsKMeansClusteringMethodResultsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DetailsKMeansClusteringResultViewModel result)
        {
            return View("_DetailsKMeansClusteringMethodResultsView", result);
        }
    }
}
