namespace DataAnalysisSystem.ServicesInterfaces
{
    public interface ICodeQRGenerator
    {
        public byte[] GenerateQRCode(string payload, string iconUrl);
    }
}
