using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class OverviewBasicStatisticsMethodViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("_OverviewBasicStatisticsMethodView");
        }
    }
}
