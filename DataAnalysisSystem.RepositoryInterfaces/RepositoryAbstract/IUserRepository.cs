using DataAnalysisSystem.DataEntities;

namespace DataAnalysisSystem.RepositoryInterfaces.RepositoryAbstract
{
    public interface IUserRepository
    {
        void UpdateUser(IdentityProviderUser user);
    }
}
