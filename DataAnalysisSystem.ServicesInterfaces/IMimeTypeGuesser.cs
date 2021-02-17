namespace DataAnalysisSystem.ServicesInterfaces
{
    public interface IMimeTypeGuesser
    {
        public string GetMimeTypeFromByteArray(byte[] fileData, string fileName);
    }
}
