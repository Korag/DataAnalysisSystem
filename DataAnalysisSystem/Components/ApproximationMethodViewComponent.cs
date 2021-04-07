using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class DeriverativeMethodViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("_DeriverativeMethodView");
        }
    }
}
