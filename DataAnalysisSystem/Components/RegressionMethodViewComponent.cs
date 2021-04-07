using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class RegressionMethodViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("_RegressionMethodView");
        }
    }
}
