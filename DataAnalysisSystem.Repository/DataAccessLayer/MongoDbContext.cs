using DataAnalysisSystem.RepositoryInterfaces.DataAccessLayerAbstract;
using DataAnalysisSystem.Services.DesignPatterns.StrategyDesignPattern.FileObjectSerializer.Serializer;
using MongoDB.Driver;
using System;
using System.IO;

namespace DataAnalysisSystem.Repository.DataAccessLayer
{
    public class MongoDbContext : DbContextAbstract
    {
        private const string PATH_AZURE_DATABASE_CREDENTIALS = "../wwwroot/DatabaseCredentials/dbCredentials.json";
        private const string PATH_LOCAL_DATABASE_CREDENTIALS = "./wwwroot/DatabaseCredentials/dbCredentials.json";

        private string _connectionString { get; set; }
        private string _databaseName { get; set; }
        private MongoClient _mongoClient { get; set; }

        public IMongoDatabase databaseInfo{ get; private set; }

        public MongoDbContext(string pathToDatabaseCredentialsFiles = PATH_LOCAL_DATABASE_CREDENTIALS)
        {
            ReadConnectionString(pathToDatabaseCredentialsFiles);
            InitializeContext(_connectionString);
        }

        public void InitializeContext(string connectionString)
        {
            _mongoClient = new MongoClient(_connectionString);
            databaseInfo = _mongoClient.GetDatabase(_databaseName);
        }

        public string ReadConnectionString(string filePath)
        {
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                var fileContent = streamReader.ReadToEnd();
                try
                {
                    dynamic deserializedObject = JsonSerializer.ConvertJsonStringToDynamicObject(fileContent);

                    _databaseName = deserializedObject["DatabaseName"];
                    _connectionString = deserializedObject["MongoAtlasConnectionString"];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return _connectionString;
        }
    }
}
