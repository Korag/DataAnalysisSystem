using DataAnalysisSystem.ServicesInterfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAnalysisSystem.Services
{
    public class FileHelper : IFileHelper
    {
        private IHostingEnvironment _environment;
        private ICodeGenerator _codeGenerator;

        public FileHelper(IHostingEnvironment environment, ICodeGenerator codeGenerator)
        {
            _environment = environment;
            _codeGenerator = codeGenerator;
        }

        public string ExtractExtensionFromFilePath(string filePath)
        {
            return Path.GetExtension(filePath).ToUpper();
        }

        public static string ReadFileContentToStringStatic(string path)
        {
            string fileText = "";

            if (File.Exists(path))
            {
                fileText = File.ReadAllText(path);
            }

            return fileText;
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
                var fileBytes = memoryStream.ToArray();
                fileText = Convert.ToBase64String(fileBytes);
            }

            return fileText;
        }

        public string SaveFileOnHardDrive(IFormFile file, string folderName)
        {
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName).ToLower();
            string generatedUniqueFileName = fileName + _codeGenerator.GenerateNewUniqueXLengthCodeAsString(4) + extension;

            var filePath = Path.Combine(_environment.WebRootPath, folderName, generatedUniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return filePath;
        }

        public string SaveStringToFileLocatedOnHardDrive(string fileContent, string fileName, string extension, string folderName)
        {
            string generatedUniqueFileName = fileName + _codeGenerator.GenerateNewUniqueXLengthCodeAsString(4) + extension;
            var filePath = Path.Combine(_environment.WebRootPath, folderName, generatedUniqueFileName);

            File.WriteAllTextAsync(filePath, fileContent);
            return filePath;
        }

        public ICollection<string> SaveFilesOnHardDrive(ICollection<IFormFile> files, string folderName)
        {
            List<string> createdFilePaths = new List<string>();

            foreach (var file in files)
            {
                createdFilePaths.Add(SaveFileOnHardDrive(file, folderName));
            }

            return createdFilePaths;
        }

        public void RemoveFileFromHardDrive(string filePath)
        {
            if (!String.IsNullOrWhiteSpace(filePath))
            {
                File.Delete(filePath);
            }
        }

        public void RemoveFilesFromHardDrive(ICollection<string> filePaths)
        {
            if (filePaths != null && filePaths.Count != 0)
            {
                foreach (var path in filePaths)
                {
                    RemoveFileFromHardDrive(path);
                }
            }
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
