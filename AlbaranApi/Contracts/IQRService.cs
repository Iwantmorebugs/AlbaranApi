using System.Drawing;
using QRCoder;

namespace AlbaranApi.Contracts
{
    public interface IQRService
    {
        QRCode CreateQrCode(string qrCodeData);
        Bitmap CreateQrImage(QRCode qrCode);
    }
}