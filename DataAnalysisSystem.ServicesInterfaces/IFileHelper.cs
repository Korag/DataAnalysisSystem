using Microsoft.AspNetCore.Http;

namespace DataAnalysisSystem.ServicesInterfaces
{
    public interface IFileHelper
    {
        public string ExtractExtensionFromFilePath(string filePath);
        public string ReadFileContentToString(string path);
        public string[] ReadFileContentToStringArray(string path);
        public byte[] ReadFileContentToByteArray(string path);
        public bool CheckIfDirectoryEmpty(string path);
        public int CountFilesInDirectory(string path);
        public byte[] ConvertIFormFileToByteArray(IFormFile file);
        public string ConvertIFormFileToString(IFormFile file);
    }
}
