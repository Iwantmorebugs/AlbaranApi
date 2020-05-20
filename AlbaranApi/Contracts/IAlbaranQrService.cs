using System.Drawing;
using AlbaranApi.Dto;
using AlbaranApi.Models;
using Inventario.EventResult.CommandResultAlbaranDto;
using QRCoder;

namespace AlbaranApi.Contracts
{
    public interface IAlbaranQrService
    {
        QRCode CreateQrCode(string qrCodeData);
        Bitmap CreateQrImage(QRCode qrCode);
        Entrada HandleAlbaranQrData(EntradaDto entradaDto);
        Entrada HandleProductQrData(Entrada entry);
        AddAmountProductAlbaranResultDto CreatePublishableResult(Entrada result);
    }
}