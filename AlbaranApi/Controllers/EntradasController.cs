using System;
using AlbaranApi.Contracts;
using AlbaranApi.Dto;
using AlbaranApi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AlbaranApi.Controllers
{
    [ApiController]
    [Route("Albaranes")]
    public class EntradasController : ControllerBase
    {
        private readonly IHandler _handler;
        private readonly ILogger<EntradasController> _logger;

        public EntradasController(
            ILogger<EntradasController> logger, IHandler handler)
        {
            _logger = logger;
            _handler = handler;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] EntradaDto entradaDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var entradaResult = _handler.HandleRegister(entradaDto);

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


        [Route("")]
        //[Route("{sortBy}/{sortOrder}")]
        [HttpGet]
        public IActionResult GetAllOrderedBy(string sortBy, string sortOrder)
        {
            var providerResult = _handler.HandleGetAll();
            //sortBy = FirstCharToUpper(sortBy);
            //var orderBy = typeof(Entrada).GetProperty(sortBy);

            //var orderedList = sortOrder == "desc" ? providerResult.OrderByDescending(x => orderBy.GetValue(x, null)).ToList() : providerResult.OrderBy(x => orderBy.GetValue(x, null)).ToList();

            return Ok(providerResult);
        }
    }
}