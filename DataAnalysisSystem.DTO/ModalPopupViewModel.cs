using System.Collections.Generic;

namespace DataAnalysisSystem.DTOViewModels
{
    public class ModalPopupViewModel
    {
        public string ModalIdentificator { get; set; }

        public string ModalTitle { get; set; }
        public string ModalContent { get; set; }

        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public Dictionary<string, string> RouteAdditionalArguments{ get; set; }

        public bool ModalActAsForm { get; set; }
        public string LabelOnButton { get; set; }
    }
}
