using DataAnalysisSystem.DataEntities;
using DataAnalysisSystem.Services.DesignPatterns.StrategyDesignPattern.FileObjectSerializer.Serializer;
using DataAnalysisSystem.ServicesInterfaces;
using DataAnalysisSystem.ServicesInterfaces.DesignPatterns.AdapterDesignPattern;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace DataAnalysisSystem.Services.DesignPatterns.AdapterDesignPattern
{
    public class XlsxSerializerAdapter : IXlsxSerializerAdapter
    {
        private XlsxSerializer _serializer;
        private ICodeGenerator _codeGenerator;
        private IWebHostEnvironment _environment;

        public XlsxSerializerAdapter(ICodeGenerator codeGenerator, IWebHostEnvironment environment)
        {
            _serializer = new XlsxSerializer();
            _codeGenerator = codeGenerator;
            _environment = environment;
        }

        public string SaveStringToFileLocatedOnHardDrive(DatasetContent datasetContent, string fileName, string fileExtension, string folderName)
        {
            string generatedUniqueFileName = fileName + _codeGenerator.GenerateNewUniqueXLengthCodeAsString(4) + fileExtension;
            var fullFilePath = Path.Combine(_environment.WebRootPath, folderName, generatedUniqueFileName);

            _serializer.SaveXlsxFileWithDatasetContent(datasetContent, fullFilePath);
            return fullFilePath;
        }
    }
}
