using DataAnalysisSystem.DataEntities;
using System.Collections.Generic;

namespace DataAnalysisSystem.RepositoryInterfaces.RepositoryAbstract
{
    public interface IDatasetRepository
    {
        public void UpdateDataset(Dataset dataset);
        public void AddDataset(Dataset dataset);
        public Dataset GetDatasetById(string datasetIdentificator);
        public ICollection<Dataset> GetDatasetsById(ICollection<string> datasetIdentificators);
    }
}
