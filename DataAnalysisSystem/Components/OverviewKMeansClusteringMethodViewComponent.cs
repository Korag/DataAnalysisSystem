using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class OverviewKMeansClusteringMethodViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("_OverviewKMeansClusteringMethodView");
        }
    }
}
