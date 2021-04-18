namespace DataAnalysisSystem.DTO.Helpers
{
    public class RegressionPredictedValueViewModel
    {
        public double OXValueFromSource { get; set; }
        public double OYValueFromSource { get; set; }

        public double OXValuePredicted { get; set; }
        public double OYValuePredicted { get; set; }
    }
}