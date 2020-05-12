using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using AlbaranApi.Contracts;
using AlbaranApi.Dto;
using AlbaranApi.Models;

namespace AlbaranApi.Handler
{
    public class Handler : IHandler
    {
        private readonly IEntradaRepository _entradaRepository;
        private readonly IQrService _qrService;

        public Handler(IEntradaRepository entradaRepository, IQrService qrService)
        {
            _entradaRepository = entradaRepository;
            _qrService = qrService;
        }

        public Entrada HandleRegister(EntradaDto entradaDto)
        {
            var entrada = EntradaDtoToEntrada(entradaDto);
            return _entradaRepository.CreateEntry(entrada);
        }

        public IEnumerable<Entrada> HandleGetAll()
        {
            return _entradaRepository.GetAllEntradas();
        }

        private Entrada EntradaDtoToEntrada(EntradaDto entradaDto)
        {
            var qrCodeData = ImageProcess(entradaDto);

            // var image = ImageToByteArray(qrImage);

            var entrada = new Entrada
            {
                QrCodeData = qrCodeData,
                EntradaId = entradaDto.EntradaId,
                ProviderId = entradaDto.ProviderId,
                ProductIdentity = entradaDto.ProductIdentity,
                ProductAmount = entradaDto.ProductAmount,
                CreationDate = entradaDto.CreationDate
            };
            return entrada;
        }

        private string ImageProcess(EntradaDto entradaDto)
        {
            var qrCodeData = entradaDto.EntradaId + "," + entradaDto.CreationDate + "," + entradaDto.ProviderId + "," +
                             entradaDto.ProductIdentity;

            var qrCode = _qrService.CreateQrCode(qrCodeData);
            var qrImage = _qrService.CreateQrImage(qrCode);

            try
            {
                qrImage.Save("QrTest" + ".jpeg", ImageFormat.Jpeg);
                qrImage.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return qrCodeData;
        }

        public static string FirstCharToUpper(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;

            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static byte[] ImageToByteArray(Image imageIn)
        {
            var imgCon = new ImageConverter();

            return (byte[]) imgCon.ConvertTo(imageIn, typeof(byte[]));
        }
    }
}