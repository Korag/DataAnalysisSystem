using DataAnalysisSystem.DTO.AnalysisDTO;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class AddKMeansClusteringMethodParametersViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PerformNewAnalysisViewModel model)
        {
            return View("_AddKMeansClusteringMethodParametersView", model);
        }
    }
}
