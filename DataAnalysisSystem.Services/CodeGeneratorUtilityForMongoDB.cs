using DataAnalysisSystem.ServicesInterfaces;
using MongoDB.Bson;
using System;

namespace DataAnalysisSystem.Services
{
    public class CodeGeneratorUtilityForMongoDB : ICodeGenerator
    {
        private ObjectId GenerateNewDbEntityUniqueIdentificator()
        {
            return ObjectId.GenerateNewId();
        }

        public ObjectId GenerateNewDbEntityUniqueIdentificatorAsObjectId()
        {
            return GenerateNewDbEntityUniqueIdentificator();
        }

        public string GenerateNewDbEntityUniqueIdentificatorAsString()
        {
            return GenerateNewDbEntityUniqueIdentificator().ToString();
        }

        private Guid GenerateNewUniqueCode()
        {
            return Guid.NewGuid();
        }

        public string GenerateNewUniqueCodeAsString()
        {
            return GenerateNewUniqueCode().ToString();
        }
    }
}
