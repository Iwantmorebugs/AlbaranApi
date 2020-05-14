using AlbaranApi.Contracts;
using AlbaranApi.Dto;
using AlbaranApi.Models;
using Inventario.EventResult.CommandResultDto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using Inventario.EventResult.CommandResultAlbaranDto;

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
            Trace();
            var entrada = MapEntradaDtoToEntrada(entradaDto);
            
            var result = _entradaRepository.CreateEntry(entrada);
            var resultToBePublished = CreatePublishableResult(result);

            if (result != null) await _domainEventResultPublisher.Consume(resultToBePublished);

            return result;
        }

        public IEnumerable<Entrada> HandleGetAll()
        {
            return _entradaRepository.GetAllEntradas();
        }

        private AddAmountProductAlbaranResultDto CreatePublishableResult(Entrada result)
        {
            var resultToBePublished = new AddAmountProductAlbaranResultDto
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
                    Message = "Entering Handler"
                };
            Console.WriteLine(trace);
        }

        private Entrada MapEntradaDtoToEntrada(EntradaDto entradaDto)
        {

            Console.WriteLine("Entering Mapper");
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
            Console.WriteLine("Entering ImageProcess");

            var qrCodeData = entradaDto.EntradaId + "," + entradaDto.CreationDate + "," + entradaDto.ProviderId + "," +
                             entradaDto.ProductIdentity;
            //try
            //{
            //    Console.WriteLine("Entering CreateQrCode");
            //    var qrCode = _qrService.CreateQrCode(qrCodeData);
            //    Console.WriteLine("Entering CreateQrImage");
            //    var qrImage = _qrService.CreateQrImage(qrCode);
            //    // qrImage.Save("QrTest" + ".jpeg", ImageFormat.Jpeg);
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