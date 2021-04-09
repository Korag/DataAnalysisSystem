using DataAnalysisSystem.DTO.AnalysisDTO;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DetailsKMeansClusteringMethodResultsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AnalysisDetailsViewModel modal)
        {
            return View("_DetailsKMeansClusteringMethodResultsView", modal);
        }
    }
}
