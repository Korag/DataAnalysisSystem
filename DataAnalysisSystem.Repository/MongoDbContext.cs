﻿using DataAnalysisSystem.RepositoryInterfaces;
using DataAnalysisSystem.Services.Serializers_Helpers;
using MongoDB.Driver;
using System;
using System.IO;

namespace DataAnalysisSystem.Repository
{
    public class MongoDbContext : DbContext
    {
        private string _connectionString { get; set; }
        private string _databaseName { get; set; }
        private MongoClient _mongoClient { get; set; }

        public IMongoDatabase databaseInfo{ get; private set; }

        public MongoDbContext(string pathToDatabaseCredentialsFiles = "../DataAnalysisSystem.Repository/dbCredentials.json")
        {
            ReadConnectionString(pathToDatabaseCredentialsFiles);
            InitializeContext(_connectionString);
        }

        public void InitializeContext(string connectionString)
        {
            _mongoClient = new MongoClient(_connectionString);
            databaseInfo = _mongoClient.GetDatabase(_databaseName);
        }

        public string ReadConnectionString(string pathToFile)
        {
            using (StreamReader streamReader = new StreamReader(pathToFile))
            {
                var fileContent = streamReader.ReadToEnd();
                try
                {
                    dynamic deserializedObject = JsonSerializerHelper.ConvertJsonStringToDynamicObject(pathToFile);

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
