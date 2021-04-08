using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class OverviewDeriverativeMethodViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("_OverviewDeriverativeMethodView");
        }
    }
}
