using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.Repository.DataAccessLayer;
using DataAnalysisSystem.RepositoryInterfaces.RepositoryAbstract;
using MongoDB.Driver;
using System.Collections.Generic;

namespace DataAnalysisSystem.Repository.Repository
{
    public class MongoAnalysisRepository : IAnalysisRepository
    {
        private readonly MongoDbContext _context;

        private readonly string _analysesCollectionName = "Analyses";
        private IMongoCollection<Analysis> _analyses;

        public MongoAnalysisRepository(MongoDbContext context)
        {
            this._context = context;
        }

        private IMongoCollection<Analysis> GetAnalyses()
        {
            return _analyses = _context.databaseInfo.GetCollection<Analysis>(_analysesCollectionName);
        }
        public Analysis GetAnalysisById(string analysisIdentificator)
        {
            var filter = Builders<Analysis>.Filter.Eq(x => x.AnalysisIdentificator, analysisIdentificator);
            var analysis = GetAnalyses().Find<Analysis>(filter).FirstOrDefault();

            return analysis;
        }

        public IList<Analysis> GetAnalysesById(IList<string> analysesIdentificators)
        {
            var filter = Builders<Analysis>.Filter.Where(x => analysesIdentificators.Contains(x.AnalysisIdentificator));
            var analyses = GetAnalyses().Find<Analysis>(filter).ToList();

            return analyses;
        }

        public IList<Analysis> GetAnalysesByDatasetId(string datasetIdentificator)
        {
            var filter = Builders<Analysis>.Filter.Where(x => x.DatasetIdentificator == datasetIdentificator);
            var analyses = GetAnalyses().Find<Analysis>(filter).ToList();

            return analyses;
        }

        public void UpdateAnalysis(Analysis analysis)
        {
            var filter = Builders<Analysis>.Filter.Eq(x => x.AnalysisIdentificator, analysis.AnalysisIdentificator);
            var result = GetAnalyses().ReplaceOne(filter, analysis);
        }

        public void AddAnalysis(Analysis analysis)
        {
            GetAnalyses().InsertOne(analysis);
        }

        public void DeleteAnalysis(string analysisIdentificator)
        {
            var filter = Builders<Analysis>.Filter.Where(z => z.AnalysisIdentificator == analysisIdentificator);
            var result = GetAnalyses().DeleteOne(filter);
        }

        public void DeleteAnalyses(IList<string> analysesIdentificators)
        {
            var filter = Builders<Analysis>.Filter.Where(z => analysesIdentificators.Contains(z.AnalysisIdentificator));
            var result = GetAnalyses().DeleteMany(filter);
        }

        public Analysis GetAnalysisByAccessKey(string accessKey)
        {
            var filter = Builders<Analysis>.Filter.Eq(x => x.AccessKey, accessKey);
            var filter2 = Builders<Analysis>.Filter.Eq(x => x.IsShared, true);

            var analysis = GetAnalyses().Find<Analysis>(filter & filter2).FirstOrDefault();

            return analysis;
        }
    }
}
