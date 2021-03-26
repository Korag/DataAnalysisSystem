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
            string code = GenerateNewUniqueCode().ToString();
            code = code.Substring(0, length);

            return code;
        }

        public string GenerateAccessKey(int length)
        {
            var randomizerText = RandomizerFactory.GetRandomizer(new FieldOptionsText { UseNumber = true, UseSpecial = false, Max = length, Min = length});
            return randomizerText.Generate();
        }
    }
}
