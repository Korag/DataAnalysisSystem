using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.Repository.DataAccessLayer;
using DataAnalysisSystem.RepositoryInterfaces.RepositoryAbstract;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace DataAnalysisSystem.Repository.Repository
{
    public class MongoUserRepository : IUserRepository
    {
        private readonly MongoDbContext _context;

        private readonly string _usersCollectionName = "Users";
        private IMongoCollection<IdentityProviderUser> _users;

        public MongoUserRepository(MongoDbContext context)
        {
            this._context = context;
        }

        private IMongoCollection<IdentityProviderUser> GetUsers()
        {
            return _users = _context.databaseInfo.GetCollection<IdentityProviderUser>(_usersCollectionName);
        }

        public IdentityProviderUser GetUserById(string userIdentificator)
        {
            var filter = Builders<IdentityProviderUser>.Filter.Eq(x => x.Id, new ObjectId(userIdentificator));
            var user = GetUsers().Find<IdentityProviderUser>(filter).FirstOrDefault();

            return user;
        }

        public IdentityProviderUser GetUserByName(string userName)
        {
            var filter = Builders<IdentityProviderUser>.Filter.Eq(x => x.UserName, userName);
            var user = GetUsers().Find<IdentityProviderUser>(filter).FirstOrDefault();

            return user;
        }

        public void UpdateUser(IdentityProviderUser user)
        {
            var filter = Builders<IdentityProviderUser>.Filter.Eq(x => x.Id, user.Id);
            var result = GetUsers().ReplaceOne(filter, user);
        }

        public void AddDatasetToOwner(string userIdentificator, string datasetIdentificator)
        {
            var user = GetUserById(userIdentificator);
            user.UserDatasets.Add(datasetIdentificator);

            UpdateUser(user);
        }

        public IdentityProviderUser RemoveDatasetFromOwner(string userIdentificator, string datasetIdentificator)
        {
            var filter = Builders<IdentityProviderUser>.Filter.Where(z => z.Id.ToString() == userIdentificator);
            var update = Builders<IdentityProviderUser>.Update.Pull(x => x.UserDatasets, datasetIdentificator);

            var resultUser = GetUsers().Find<IdentityProviderUser>(filter).FirstOrDefault();
            var result = _users.UpdateOne(filter, update);

            return resultUser;
        }

        public IList<IdentityProviderUser> RemoveSharedDatasetsFromUsers(string datasetIdentificator)
        {
            var filter = Builders<IdentityProviderUser>.Filter.Where(z => z.SharedDatasetsToUser.Contains(datasetIdentificator));
            var update = Builders<IdentityProviderUser>.Update.Pull(x => x.SharedDatasetsToUser, datasetIdentificator);

            var resultListOfUsers = GetUsers().Find<IdentityProviderUser>(filter).ToList();

            var result = _users.UpdateMany(filter, update);

            return resultListOfUsers;
        }

        public IList<IdentityProviderUser> RemoveSharedAnalysesFromUsers(IList<string> analysesIdentificators)
        {
            var filter = Builders<IdentityProviderUser>.Filter.Where(z => z.SharedAnalysesToUser.ToList().Where(x => analysesIdentificators.Contains(x)).ToList().Count() != 0);
            var update = Builders<IdentityProviderUser>.Update.PullAll(x => x.SharedAnalysesToUser, analysesIdentificators);

            var resultListOfUsers = GetUsers().Find<IdentityProviderUser>(filter).ToList();

            return resultListOfUsers;
        }

        public IdentityProviderUser RemoveAnalysesFromOwner(string userIdentificator, IList<string> analysesIdentificators)
        {
            var filter = Builders<IdentityProviderUser>.Filter.Where(z => z.Id.ToString() == userIdentificator);
            var update = Builders<IdentityProviderUser>.Update.PullAll(x => x.UserAnalyses, analysesIdentificators);

            var resultUser = GetUsers().Find<IdentityProviderUser>(filter).FirstOrDefault();
            var result = _users.UpdateOne(filter, update);

            return resultUser;
        }
    }
}
