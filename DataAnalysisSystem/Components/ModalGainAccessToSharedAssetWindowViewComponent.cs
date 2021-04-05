using DataAnalysisSystem.DTO.AdditionalFunctionalities;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class ModalGainAccessToSharedAssetWindowViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ModalGainAccessToSharedAssetWindowViewModel modal)
        {
            return View("_ModalGainAccessToSharedAsset", modal);
        }
    }
}
