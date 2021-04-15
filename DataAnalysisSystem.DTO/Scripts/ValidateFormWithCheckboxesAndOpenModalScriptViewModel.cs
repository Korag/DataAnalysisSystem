using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.Scripts
{
    public class ValidateFormWithCheckboxesAndOpenModalScriptViewModel
    {
        public ValidateFormWithCheckboxesAndOpenModalScriptViewModel()
        {
            this.CheckboxesToLimitValidation = new List<CheckboxLimiterValidationScriptViewModel>();
        }

        public string FormId { get; set; }
        public string ModalId { get; set; }
        public string ValidationContainerId { get; set; }
        public IList<CheckboxLimiterValidationScriptViewModel> CheckboxesToLimitValidation { get; set; }
    }
}
