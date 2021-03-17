using DataAnalysisSystem.ServicesInterfaces;
using QRCoder;
using System.Drawing;
using System.IO;

namespace DataAnalysisSystem.Services
{
    public class CodeQRGenerator : ICodeQRGenerator
    {
        public byte[] GenerateQRCode(string payload, string iconUrl)
        {
            QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            Bitmap icon = new Bitmap(iconUrl, true);
            Bitmap qrCodeBitmap = qrCode.GetGraphic(20, Color.Black, Color.White, icon);

            using (MemoryStream stream = new MemoryStream())
            {
                qrCodeBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
