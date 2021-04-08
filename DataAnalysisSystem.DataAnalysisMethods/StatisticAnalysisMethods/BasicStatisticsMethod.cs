using DataAnalysisSystem.DataEntities;

namespace DataAnalysisSystem.DataAnalysisMethods
{
    public class BasicStatisticsMethod : IAnalysisMethod
    {
        public AnalysisResults GetDataAnalysisMethodResult(DatasetContent datasetContent, AnalysisParameters parameters)
        {
            var a = 0;
            AnalysisResults ar = new AnalysisResults();
            ar.BasicStatisticsResult = new BasicStatisticsResult();
            return ar;
        }
    }
}
