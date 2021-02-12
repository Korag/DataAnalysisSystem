using DataAnalysisSystem.DTO.AdditionalFunctionalities;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class ModalPopupWindowViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ModalPopupViewModel modal)
        {
            return View("_ModalPopup", modal);
        }
    }
}
