using DataAnalysisSystem.DTO.AnalysisParametersDTO.ParametersDetails;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DetailsKMeansClusteringMethodParametersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DetailsKMeansClusteringParametersViewModel parameters)
        {
            return View("_DetailsKMeansClusteringMethodParametersView", parameters);
        }
    }
}
