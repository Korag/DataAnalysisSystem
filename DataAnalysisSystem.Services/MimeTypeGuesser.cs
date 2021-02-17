using DataAnalysisSystem.ServicesInterfaces;
using HeyRed.Mime;

namespace DataAnalysisSystem.Services
{
    public class MimeTypeGuesser : IMimeTypeGuesser
    {
        public string GetMimeTypeFromByteArray(byte[] fileData, string fileName)
        {
            var contentType = MimeGuesser.GuessMimeType(fileData);

            if (string.IsNullOrWhiteSpace(contentType))
            {
                return MimeTypesMap.GetMimeType(fileName);
            }

            return contentType;
        }
    }
}
