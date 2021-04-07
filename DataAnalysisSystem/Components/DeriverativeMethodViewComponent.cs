using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class ApproximationMethodViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("_ApproximationMethodView");
        }
    }
}
