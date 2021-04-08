using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class OverviewRegressionMethodViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("_OverviewRegressionMethodView");
        }
    }
}
