using DataAnalysisSystem.DataEntities;
using MongoDB.Bson;
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
        public IdentityProviderUser GetDatasetOwnerByDatasetId(string datasetIdentificator);
        public IdentityProviderUser GetAnalysisOwnerByAnalysisId(string analysisIdentificator);
        public IList<IdentityProviderUser> GetUsersSharedDatasetBySharedDatasetId(string datasetIdentificator);
        public IList<IdentityProviderUser> RemoveSharedAnalysisFromUsers(string analysisIdentificator);
        public IdentityProviderUser RemoveAnalysisFromOwner(ObjectId userIdentificator, string analysisIdentificator);
    }
}
