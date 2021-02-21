using DataAnalysisSystem.DataEntities;

namespace DataAnalysisSystem.RepositoryInterfaces.RepositoryAbstract
{
    public interface IUserRepository
    {
        public IdentityProviderUser GetUserById(string userIdentificator);
        public IdentityProviderUser GetUserByName(string userName);
        void UpdateUser(IdentityProviderUser user);
        public void AddDatasetToOwner(string userIdentificator, string datasetIdentificator);
    }
}
