using DataAnalysisSystem.DTO.AdditionalFunctionalities;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalysisSystem.Components
{
    public class ModalGainAccessToSharedDatasetWindowViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ModalGainAccessToSharedDatasetWindowViewModel modal)
        {
            return View("_ModalGainAccessToSharedDataset", modal);
        }
    }
}
