using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class KMeansClusteringMethodViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("_KMeansClusteringMethodView");
        }
    }
}
