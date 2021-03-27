using DataAnalysisSystem.DataEntities;

namespace DataAnalysisSystem.ServicesInterfaces.DesignPatterns.AdapterDesignPattern
{
    public interface IXlsxSerializerAdapter
    {
        public string SaveStringToFileLocatedOnHardDrive(DatasetContent datasetContent, string fileName, string fileExtension, string folderName);
    }
}
