using DataAnalysisSystem.DataEntities;

namespace DataAnalysisSystem.DataAnalysisMethods
{
    public interface IAnalysisMethod
    {
        AnalysisResults GetDataAnalysisMethodResult(DatasetContent datasetContent, AnalysisParameters parameters);
    }
}
