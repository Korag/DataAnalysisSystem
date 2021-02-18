using DataAnalysisSystem.ServicesInterfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace DataAnalysisSystem.Services
{
    public class FileHelper : IFileHelper
    {
        private IHostingEnvironment _environment;

        public FileHelper(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public string ExtractExtensionFromFilePath(string filePath)
        {
            return Path.GetExtension(filePath).ToUpper();
        }

        public string ReadFileContentToString(string path)
        {
            string fileText = "";

            if (File.Exists(path))
            {
                fileText = File.ReadAllText(path);
            }

            return fileText;
        }

        public string[] ReadFileContentToStringArray(string path)
        {
            string[] fileLines = { };

            if (File.Exists(path))
            {
                fileLines = File.ReadAllLines(path);
            }

            return fileLines;
        }

        public byte[] ReadFileContentToByteArray(string path)
        {
            byte[] fileBytes = { };

            if (File.Exists(path))
            {
                fileBytes = File.ReadAllBytes(path);
            }

            return fileBytes;
        }

        public byte[] ConvertIFormFileToByteArray(IFormFile file)
        {
            byte[] fileBytes = { };

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            return fileBytes;
        }

        public string ConvertIFormFileToString(IFormFile file)
        {
            string fileText = "";

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                fileText = memoryStream.ToString();
            }

            return fileText;
        }

        public bool CheckIfDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public int CountFilesInDirectory(string path)
        {
            return Directory.GetFiles(path).Count();
        }
    }
}
