using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class BasicStatisticsMethodViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("_BasicStatisticsMethodView");
        }
    }
}
