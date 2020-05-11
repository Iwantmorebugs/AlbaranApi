using System.Drawing;
using QRCoder;

namespace AlbaranApi.Contracts
{
    public interface IQrService
    {
        QRCode CreateQrCode(string qrCodeData);
        Bitmap CreateQrImage(QRCode qrCode);
    }
}