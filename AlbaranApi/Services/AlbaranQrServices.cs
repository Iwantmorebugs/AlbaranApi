using AlbaranApi.Contracts;
using AlbaranApi.Dto;
using AlbaranApi.Models;
using Inventario.EventResult.CommandResultAlbaranDto;
using Inventario.EventResult.Model;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;


namespace AlbaranApi.Services
{
    public class AlbaranQrServices : IAlbaranQrService
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

        public Entrada HandleAlbaranQrData(EntradaDto entradaDto)
        {
            var entrada = MapEntradaDtoToEntrada(entradaDto);
            var qrCodeData = GetAlbaranQrData(entradaDto);
            entrada.AlbaranQrCodeData = qrCodeData;
            return entrada;
        }

        private EntradaProducto MapEntradaProductoDtoToProducto(EntradaProductoDto entradaDto)
        {
            var entradaProducto = new EntradaProducto
            {
                ProductIdentity = entradaDto.ProductIdentity,
                ProductName = entradaDto.ProductName,
                ProductAmount = entradaDto.ProductAmount,
                ProductPrice = entradaDto.ProductPrice,
                ProductQrData = "",
                Trace = ""
            };

            return entradaProducto;
        }


        public string GetAlbaranQrData(EntradaDto entradaDto)
        {
            Console.WriteLine("Entering GetAlbaranQrData");

            var qrCodeData = entradaDto.EntradaId + "," + entradaDto.CreationDate + "," + entradaDto.ProviderId;
         
           // GenerateImage(entradaDto.Id.ToString(), qrCodeData);

            return qrCodeData;
        }

        private void GenerateImage(string id, string qrCodeData)
        {
            try
            {
                Console.WriteLine("Entering CreateQrCode");
                var qrCode = CreateQrCode(qrCodeData);
                Console.WriteLine("Entering CreateQrImage");
                var qrImage = CreateQrImage(qrCode);
                qrImage.Save(id + ".jpeg", ImageFormat.Jpeg);
                qrImage.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public AddAmountProductAlbaranResultDto CreatePublishableResult(Entrada result)
        {
            var listaProductosInventariosResult = new List<ProductoInventario>();
            foreach (var prod in result.EntradaProductos)
            {
                listaProductosInventariosResult.Add(new ProductoInventario
                {
                    ProductIdentity = prod.ProductIdentity, ProductAmount = prod.ProductAmount,
                });
            }

            var resultToBePublished = new AddAmountProductAlbaranResultDto
            {
                EntradaProductos = listaProductosInventariosResult
            };

            return resultToBePublished;
        }

        private Entrada MapEntradaDtoToEntrada(EntradaDto entradaDto)
        {
            Console.WriteLine("Entering Mapper");
            // var image = ImageToByteArray(qrImage);
            var entrada = new Entrada
            {
                AlbaranQrCodeData = "",
                Id = entradaDto.EntradaId,
                ProviderName = entradaDto.ProviderName,
                ProviderId = entradaDto.ProviderId,
                CreationDate = entradaDto.CreationDate,
                Observation = entradaDto.Observation,
                EntradaProductos = 
                    entradaDto.EntradaProductosDto
                        .Select(prod => MapEntradaProductoDtoToProducto(prod))
                        .ToList()
            };

            return entrada;
        }

        private static string FirstCharToUpper(string s)
        {
            if (String.IsNullOrEmpty(s)) return String.Empty;

            return Char.ToUpper(s[0]) + s.Substring(1);
        }
        public Entrada HandleProductQrData(Entrada entry)
        {
            var entrada = entry;

            foreach (var producto in entrada.EntradaProductos)
            {
                var productTrace = WeekOfYearIso8601(DateTime.Now) + "/" + entrada.ProviderId + "/" +
                                   producto.ProductIdentity + "/" + producto.ProductName;

                producto.Trace = productTrace;

                var productQrData = "ID producto: "+ producto.ProductIdentity + "/" + "Nombre producto: " + producto.ProductName + "/"+ "Cantidad producto: " +producto.ProductAmount +
                                    "/"+ "Id Proveedor: " + entrada.ProviderId;

                producto.ProductQrData = productQrData;

              //  GenerateImage(producto.ProductIdentity.ToString(), productQrData);
            }

            return entrada;
        }
        private static byte[] ImageToByteArray(Image imageIn)
        {
            var imgCon = new ImageConverter();

            return (byte[])imgCon.ConvertTo(imageIn, typeof(byte[]));
        }

        private static int WeekOfYearIso8601(DateTime date)
        {
            var day = (int)CultureInfo.CurrentCulture.Calendar.GetDayOfWeek(date);
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date.AddDays(4 - (day == 0 ? 7 : day)),
                CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}