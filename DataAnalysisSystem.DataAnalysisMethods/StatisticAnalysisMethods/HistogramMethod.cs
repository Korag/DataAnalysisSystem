using DataAnalysisSystem.DataEntities;

namespace DataAnalysisSystem.DataAnalysisMethods
{
    public class HistogramMethod : IAnalysisMethod
    {
        public AnalysisResults GetDataAnalysisMethodResult(DatasetContent datasetContent, AnalysisParameters parameters)
        {
            var b = 0;
            AnalysisResults ar = new AnalysisResults();
            ar.HistogramResult = new HistogramResult();
            return ar;
        }
    }
}
