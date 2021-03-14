using DataAnalysisSystem.ServicesInterfaces;
using MongoDB.Bson;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;
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

        public string GenerateNewUniqueXLengthCodeAsString(int length)
        {
            return GenerateNewUniqueCode().ToString().Substring(length);
        }

        public string GenerateAccessKey(int length)
        {
            var randomizerText = RandomizerFactory.GetRandomizer(new FieldOptionsText { UseNumber = true, UseSpecial = false, Max = length, Min = length});
            return randomizerText.Generate();
        }
    }
}
