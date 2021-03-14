using DataAnalysisSystem.DataEntities;
using System.Collections.Generic;

namespace DataAnalysisSystem.RepositoryInterfaces.RepositoryAbstract
{
    public interface IDatasetRepository
    {
        public void UpdateDataset(Dataset dataset);
        public void AddDataset(Dataset dataset);
        public Dataset GetDatasetById(string datasetIdentificator);
        public IList<Dataset> GetDatasetsById(ICollection<string> datasetIdentificators);
        public void DeleteDataset(string datasetIdentificator);
    }
}
