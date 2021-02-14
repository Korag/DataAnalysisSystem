using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.Repository.DataAccessLayer;
using DataAnalysisSystem.RepositoryInterfaces.RepositoryAbstract;
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

        public void UpdateUser(IdentityProviderUser user)
        {
            var filter = Builders<IdentityProviderUser>.Filter.Eq(x => x.Id, user.Id);
            var result = GetUsers().ReplaceOne(filter, user);
        }
    }
}
