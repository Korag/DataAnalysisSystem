using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class OverviewHistogramMethodViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("_OverviewHistogramMethodView");
        }
    }
}
