using AspNetCore.Identity.Mongo.Model;

namespace DataAnalysisSystem.DataEntities
{
    public class IdentityProviderUser : MongoUser
    {
        public IdentityProviderUser() : base()
        {

        }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
