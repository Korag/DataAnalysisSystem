using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class HistogramMethodViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("_HistogramMethodView");
        }
    }
}
