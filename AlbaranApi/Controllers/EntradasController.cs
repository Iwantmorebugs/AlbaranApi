using System;
using System.Drawing;
using System.IO;
using AlbaranApi.Contracts;
using AlbaranApi.Dto;
using AlbaranApi.Models;
using AlbaranApi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QRCoder;

namespace AlbaranApi.Controllers
{
    [ApiController]
    [Route("Albaranes")]
    public class EntradasController : ControllerBase
    {
        private readonly IEntradaRepository _entradasRepository;
        private readonly IQRService _qrService;

        private readonly ILogger<EntradasController> _logger;

        public EntradasController(
            ILogger<EntradasController> logger,
            IEntradaRepository entradasRepository,
            IQRService qrService)
        {
            _logger = logger;
            _entradasRepository = entradasRepository;
            _qrService = qrService;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] EntradaDto entradaDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var entrada = EntradaDtoToEntrada(entradaDto);
                var entradaResult = _entradasRepository.CreateEntry(entrada);

                return Ok(entradaResult);
            }
            catch (EntradaNotCreatedException)
            {
                return Conflict();
            }
            catch (Exception)
            {
                return StatusCode(500, "Server Error Occured");
            }
        }

        private Entrada EntradaDtoToEntrada(EntradaDto entradaDto)
        {
            var qrCodeData = entradaDto.EntradaId + "," + entradaDto.CreationDate + "," + entradaDto.ProviderId + "," +
                             entradaDto.ProductIdentity;
            QRCode qrCode = _qrService.CreateQrCode(qrCodeData);
            Bitmap qrImage = _qrService.CreateQrImage(qrCode);

            var image = ImageToByteArray(qrImage);

            var entrada = new Entrada
            {
                QrCodeData = qrCodeData,
                EntradaId = entradaDto.EntradaId,
                ProviderId = entradaDto.ProviderId,
                ProductIdentity = entradaDto.ProductIdentity,
                ProductAmount = entradaDto.ProductAmount,
                CreationDate = entradaDto.CreationDate,
                QrCodeImage = image

            };
            return entrada;
        }

        [Route("")]
        //[Route("{sortBy}/{sortOrder}")]
        [HttpGet]
        public IActionResult GetAllOrderedBy(string sortBy, string sortOrder)
        {
            var providerResult = _entradasRepository.GetAllEntradas();
            //sortBy = FirstCharToUpper(sortBy);
            //var orderBy = typeof(Entrada).GetProperty(sortBy);

            //var orderedList = sortOrder == "desc" ? providerResult.OrderByDescending(x => orderBy.GetValue(x, null)).ToList() : providerResult.OrderBy(x => orderBy.GetValue(x, null)).ToList();

            return Ok(providerResult);
        }

        public static string FirstCharToUpper(string s)
        {
            // Check for empty string.  
            if (string.IsNullOrEmpty(s)) return string.Empty;
            // Return char and concat substring.  
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            ImageConverter imgCon = new ImageConverter();
            return (byte[])imgCon.ConvertTo(imageIn, typeof(byte[]));
        }
    }
}