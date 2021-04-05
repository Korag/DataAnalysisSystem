using AspNetCore.Identity.Mongo.Model;
using System.Collections.Generic;

namespace DataAnalysisSystem.DataEntities
{
    public class IdentityProviderUser : MongoUser
    {
        public IdentityProviderUser() : base()
        {
            this.UserDatasets = new List<string>();
            this.UserAnalyses = new List<string>();
            this.SharedDatasetsToUser = new List<string>();
            this.SharedAnalysesToUser = new List<string>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IList<string> UserDatasets { get; set; }
        public IList<string> UserAnalyses { get; set; }
        public IList<string> SharedDatasetsToUser { get; set; }
        public IList<string> SharedAnalysesToUser { get; set; }
    }
}
