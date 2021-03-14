using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.Repository.DataAccessLayer;
using DataAnalysisSystem.RepositoryInterfaces.RepositoryAbstract;
using MongoDB.Driver;
using System.Collections.Generic;

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

        public Dataset GetDatasetById(string datasetIdentificator)
        {
            var filter = Builders<Dataset>.Filter.Eq(x => x.DatasetIdentificator, datasetIdentificator);
            var dataset = GetDatasets().Find<Dataset>(filter).FirstOrDefault();

            return dataset;
        }

        public ICollection<Dataset> GetDatasetsById(ICollection<string> datasetIdentificators)
        {
            var filter = Builders<Dataset>.Filter.Where(x => datasetIdentificators.Contains(x.DatasetIdentificator));
            var datasets = GetDatasets().Find<Dataset>(filter).ToList();

            return datasets;
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

        public void DeleteDataset(string datasetIdentificator)
        {
            var filter = Builders<Dataset>.Filter.Where(z => z.DatasetIdentificator == datasetIdentificator);
            var result = GetDatasets().DeleteOne(filter);
        }
    }
}
