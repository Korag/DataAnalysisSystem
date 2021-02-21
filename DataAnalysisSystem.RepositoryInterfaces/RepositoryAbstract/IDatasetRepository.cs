using DataAnalysisSystem.DataEntities;

namespace DataAnalysisSystem.RepositoryInterfaces.RepositoryAbstract
{
    public interface IDatasetRepository
    {
        public void UpdateDataset(Dataset dataset);
        public void AddDataset(Dataset dataset);
    }
}
