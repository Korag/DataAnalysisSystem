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

        public ICollection<string> UserDatasets { get; set; }
        public ICollection<string> UserAnalyses { get; set; }
        public ICollection<string> SharedDatasetsToUser { get; set; }
        public ICollection<string> SharedAnalysesToUser { get; set; }
    }
}
