using DataAnalysisSystem.DTO.AdditionalFunctionalities;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class ModalBottomSheetWindowViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ModalBottomSheetViewModel modal)
        {
            return View("_ModalBottomSheet", modal);
        }
    }
}
