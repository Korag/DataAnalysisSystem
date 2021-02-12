using MongoDB.Bson;

namespace DataAnalysisSystem.ServicesInterfaces
{
    public interface ICodeGenerator
    {
        public string GenerateNewDbEntityUniqueIdentificatorAsString();
        public ObjectId GenerateNewDbEntityUniqueIdentificatorAsObjectId();
        public string GenerateNewUniqueCodeAsString();
    }
}
