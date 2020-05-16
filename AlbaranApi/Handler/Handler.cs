using AlbaranApi.Contracts;
using AlbaranApi.Dto;
using AlbaranApi.Models;
using Inventario.EventResult.CommandResultAlbaranDto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Inventario.EventResult.Model;
using Container = Inventario.EventResult.Model.Container;

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
            HandleRegisterTrace();
            var entrada = HandleDatosAlbaran(entradaDto);
            HandleProductQrData(entrada);
            await _entradaRepository.CreateEntry(entrada);
            var resultToBePublished = CreatePublishableResult(entrada);
            if (entrada != null) await _domainEventResultPublisher.Consume(resultToBePublished);

            return entrada;
        }

        private static void HandleProductQrData(Entrada entrada)
        {
            foreach (var producto in entrada.EntradaProductos)
            {
                var productTrace = WeekOfYearIso8601(DateTime.Now) + "/" + entrada.ProviderId + "/" +
                                   producto.ProductIdentity + "/" + producto.ProductName;

                producto.Trace = productTrace;

                var productQrData = producto.ProductIdentity + "/" + producto.ProductName + producto.ProductAmount +
                                    "/" + entrada.ProviderId;

                producto.ProductQrData = productQrData;
            }
        }

        private Entrada HandleDatosAlbaran(EntradaDto entradaDto)
        {
            var entrada = MapEntradaDtoToEntrada(entradaDto);
            var qrCodeData = GetAlbaranQrData(entradaDto);
            entrada.AlbaranQrCodeData = qrCodeData;
            return entrada;
        }

        public  async  Task<IEnumerable<Entrada>>   HandleGetAll()
        {
            var result =  await _entradaRepository.GetAllEntradas();
            return result;
        }

        private AddAmountProductAlbaranResultDto CreatePublishableResult(Entrada result)
        {
            List<ProductoInventario> listaProductosInventariosResult =
                (from prod in result.EntradaProductos
                    let listaContainersInventariosResult =
                        prod.Container.Select(container => new Container()
                        {
                            Amount = container.Amount, ContainerType = container.ContainerType,
                            ProductIdentity = container.ProductIdentity
                        }).ToList()
                    select new ProductoInventario()
                    {
                        ProductIdentity = prod.ProductIdentity, ProductAmount = prod.ProductAmount,
                        Containers = listaContainersInventariosResult
                    }).ToList();

            var resultToBePublished = new AddAmountProductAlbaranResultDto
            {
                EntradaProductos = listaProductosInventariosResult
            };

            return resultToBePublished;
        }

        private static void HandleRegisterTrace()
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
            // var image = ImageToByteArray(qrImage);
            var entrada = new Entrada
            {
                AlbaranQrCodeData = "",
                EntradaId = entradaDto.EntradaId,
                ProviderId = entradaDto.ProviderId,
                CreationDate = entradaDto.CreationDate,
                Observation = entradaDto.Observation,
                EntradaProductos = entradaDto.EntradaProductosDto.Select(prod => MapEntradaProductoDtoToProducto(prod)).ToList()
            };

            return entrada;
        }

        private EntradaProducto MapEntradaProductoDtoToProducto(EntradaProductoDto entradaDto)
        {
            var entradaProducto = new EntradaProducto()
            {
                Container = entradaDto.Container,
                ProductIdentity = entradaDto.ProductIdentity,
                ProductName = entradaDto.ProductName,
                ProductAmount = entradaDto.ProductAmount,
                ProductPrice = entradaDto.ProductPrice,
                ProductQrData = "",
                Trace = "",
            };

            return entradaProducto;
        }

        private string GetAlbaranQrData(EntradaDto entradaDto)
        {
            Console.WriteLine("Entering GetAlbaranQrData");

            var qrCodeData = entradaDto.EntradaId + "," + entradaDto.CreationDate + "," + entradaDto.ProviderId;
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

        private static string FirstCharToUpper(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;

            return char.ToUpper(s[0]) + s.Substring(1);
        }

        private static byte[] ImageToByteArray(Image imageIn)
        {
            var imgCon = new ImageConverter();

            return (byte[]) imgCon.ConvertTo(imageIn, typeof(byte[]));
        }

        private static int WeekOfYearIso8601(DateTime date)
        {
            var day = (int)CultureInfo.CurrentCulture.Calendar.GetDayOfWeek(date);
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date.AddDays(4 - (day == 0 ? 7 : day)), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}