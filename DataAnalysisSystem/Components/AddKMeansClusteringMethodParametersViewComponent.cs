using DataAnalysisSystem.DTO.AnalysisParametersDTO.AddParameters;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class AddKMeansClusteringMethodParametersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AddKMeansClusteringParametersViewModel parameters)
        {
            return View("_AddKMeansClusteringMethodParametersView", parameters);
        }
    }
}
