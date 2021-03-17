using DataAnalysisSystem.DataEntities;
using System.Collections.Generic;

namespace DataAnalysisSystem.RepositoryInterfaces.RepositoryAbstract
{
    public interface IUserRepository
    {
        public IdentityProviderUser GetUserById(string userIdentificator);
        public IdentityProviderUser GetUserByName(string userName);
        void UpdateUser(IdentityProviderUser user);
        public void AddDatasetToOwner(string userIdentificator, string datasetIdentificator);
        public IdentityProviderUser RemoveDatasetFromOwner(string userIdentificator, string datasetIdentificator);
        public IList<IdentityProviderUser> RemoveSharedDatasetsFromUsers(string datasetIdentificator);
        public IList<IdentityProviderUser> RemoveSharedAnalysesFromUsers(IList<string> analysesIdentificators);
        public IdentityProviderUser RemoveAnalysesFromOwner(string userIdentificator, IList<string> analysesIdentificators);
        public IdentityProviderUser GetUserByDatasetId(string datasetIdentificator);
    }
}
