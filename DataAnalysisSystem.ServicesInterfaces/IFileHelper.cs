using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

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
        public string SaveFileOnHardDrive(IFormFile file, string folderName);
        public ICollection<string> SaveFilesOnHardDrive(ICollection<IFormFile> files, string folderName);
        public string SaveStringToFileLocatedOnHardDrive(string fileContent, string fileName, string extension, string folderName);
        public void RemoveFileFromHardDrive(string filePath);
        public void RemoveFilesFromHardDrive(ICollection<string> filePaths);

    }
}
