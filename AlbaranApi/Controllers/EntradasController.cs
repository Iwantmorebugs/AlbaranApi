using AlbaranApi.Contracts;
using AlbaranApi.Dto;
using AlbaranApi.Models;
using AlbaranApi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace AlbaranApi.Controllers
{
    [ApiController]
    [Route("Albaranes")]
    public class EntradasController : ControllerBase
    {
        private readonly IEntradaRepository _entradasRepository;

        private readonly ILogger<EntradasController> _logger;

        public EntradasController(ILogger<EntradasController> logger, IEntradaRepository entradasRepository)
        {
            _logger = logger;
            _entradasRepository = entradasRepository;
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

        private static Entrada EntradaDtoToEntrada(EntradaDto entradaDto)
        {
            var entrada = new Entrada()
            {
                QrCodeData = entradaDto.EntradaId + "," + entradaDto.CreationDate + "," + entradaDto.ProviderId + "," +
                             entradaDto.ProductIdentity,
                EntradaId = entradaDto.EntradaId,
                ProviderId = entradaDto.ProviderId,
                ProductIdentity = entradaDto.ProductIdentity,
                ProductAmount = entradaDto.ProductAmount,
                CreationDate = entradaDto.CreationDate
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
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.  
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
