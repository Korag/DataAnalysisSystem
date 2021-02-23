using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.Repository.DataAccessLayer;
using DataAnalysisSystem.RepositoryInterfaces.RepositoryAbstract;
using MongoDB.Bson;
using MongoDB.Driver;

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
    }
}
