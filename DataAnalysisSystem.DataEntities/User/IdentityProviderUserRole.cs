using AspNetCore.Identity.Mongo.Model;

namespace DataAnalysisSystem.DataEntities
{
    public class IdentityProviderUserRole : MongoRole
    {
        public IdentityProviderUserRole()
        {

        }

        public IdentityProviderUserRole(string name)
        {
            Name = name;
            NormalizedName = name.ToUpperInvariant();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
