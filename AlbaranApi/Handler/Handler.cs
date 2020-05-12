using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using AlbaranApi.Contracts;
using AlbaranApi.Dto;
using AlbaranApi.Models;
using Inventario.EventResult.CommandResultDto;

namespace AlbaranApi.Handler
{
    public class Handler : IHandler
    {
        private readonly IDomainEventResultPublisher _domainEventResultPublisher;
        private readonly IEntradaRepository _entradaRepository;
        private readonly IQrService _qrService;

        public Handler(IEntradaRepository entradaRepository, IQrService qrService,
            IDomainEventResultPublisher domainEventResultPublisher)
        {
            _entradaRepository = entradaRepository;
            _qrService = qrService;
            _domainEventResultPublisher = domainEventResultPublisher;
        }

        public async Task<Entrada> HandleRegister(EntradaDto entradaDto)
        {
            var entrada = MapEntradaDtoToEntrada(entradaDto);
            Trace();

            var result = _entradaRepository.CreateEntry(entrada);
            var resultToBePublished = CreatePublishableResult(result);

            if (result != null) await _domainEventResultPublisher.Consume(resultToBePublished);

            return result;
        }

        public IEnumerable<Entrada> HandleGetAll()
        {
            return _entradaRepository.GetAllEntradas();
        }

        private AddAmountProductResultDto CreatePublishableResult(Entrada result)
        {
            var resultToBePublished = new AddAmountProductResultDto
            {
                ExistenciaProductoId = result.ProductIdentity,
                ProductName = result.ProductName,
                TotalResult = result.ProductAmount,
                price = result.ProductPrice,
                Picture = result.Picture,
                Category = result.Category,
                Brand = result.Brand
            };
            return resultToBePublished;
        }

        private static void Trace()
        {
            var trace =
                new
                {
                    Time = DateTime.Now,
                    Message = "Receiving Entry command"
                };
            Console.WriteLine(trace);
        }

        private Entrada MapEntradaDtoToEntrada(EntradaDto entradaDto)
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
                CreationDate = entradaDto.CreationDate,
                ProductName = entradaDto.ProductName,
                ProductPrice = entradaDto.ProductPrice,
                Brand = entradaDto.Brand,
                Picture = entradaDto.Picture,
                Category = entradaDto.Category
            };
            return entrada;
        }

        private string ImageProcess(EntradaDto entradaDto)
        {
            var qrCodeData = entradaDto.EntradaId + "," + entradaDto.CreationDate + "," + entradaDto.ProviderId + "," +
                             entradaDto.ProductIdentity;

            var qrCode = _qrService.CreateQrCode(qrCodeData);
            var qrImage = _qrService.CreateQrImage(qrCode);

            //try
            //{
            //    qrImage.Save("QrTest" + ".jpeg", ImageFormat.Jpeg);
            //    qrImage.Dispose();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}

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