using DataAnalysisSystem.DataEntities;
using System.Collections.Generic;

namespace DataAnalysisSystem.RepositoryInterfaces.RepositoryAbstract
{
    public interface IAnalysisRepository
    {
        public Analysis GetAnalysisById(string analysisIdentificator);
        public IList<Analysis> GetAnalysesById(IList<string> analysesIdentificators);

        public IList<Analysis> GetAnalysesByDatasetId(string datasetIdentificator);
        public void UpdateAnalysis(Analysis analysis);
        public void AddAnalysis(Analysis analysis);
        public void DeleteAnalysis(string analysisIdentificator);
        public void DeleteAnalyses(IList<string> analysesIdentificators);
        public Analysis GetAnalysisByAccessKey(string accessKey);
    }
}
