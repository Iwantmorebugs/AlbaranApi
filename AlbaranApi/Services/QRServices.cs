using System.Drawing;
using AlbaranApi.Contracts;
using QRCoder;

namespace AlbaranApi.Services
{
    public class QrServices : IQrService
    {
        public QRCode CreateQrCode(string qrData)
        {
            var qrGenerator = new QRCodeGenerator();

            var qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);

            return new QRCode(qrCodeData);
        }

        public Bitmap CreateQrImage(QRCode qrCode)
        {
            return qrCode.GetGraphic(20);
        }
    }
}