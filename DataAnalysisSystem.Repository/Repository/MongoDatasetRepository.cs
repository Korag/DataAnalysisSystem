using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.Repository.DataAccessLayer;
using DataAnalysisSystem.RepositoryInterfaces.RepositoryAbstract;
using MongoDB.Driver;

namespace DataAnalysisSystem.Repository.Repository
{
    public class MongoDatasetRepository : IDatasetRepository
    {
        private readonly MongoDbContext _context;

        private readonly string _datasetsCollectionName = "Datasets";
        private IMongoCollection<Dataset> _datasets;

        public MongoDatasetRepository(MongoDbContext context)
        {
            this._context = context;
        }

        private IMongoCollection<Dataset> GetDatasets()
        {
            return _datasets = _context.databaseInfo.GetCollection<Dataset>(_datasetsCollectionName);
        }

        public void UpdateDataset(Dataset dataset)
        {
            var filter = Builders<Dataset>.Filter.Eq(x => x.DatasetIdentificator, dataset.DatasetIdentificator);
            var result = GetDatasets().ReplaceOne(filter, dataset);
        }

        public void AddDataset(Dataset dataset)
        {
            GetDatasets().InsertOne(dataset);
        }
    }
}
