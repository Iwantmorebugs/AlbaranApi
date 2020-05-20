using AlbaranApi.Contracts;
using AlbaranApi.Dto;
using AlbaranApi.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlbaranApi.Models;

namespace AlbaranApi.Controllers
{
    [ApiController]
    [Route("Albaranes")]
    public class EntradasController : ControllerBase
    {
        private readonly IHandler _handler;

        public EntradasController(
            IHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] EntradaDto entradaDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                Console.WriteLine("Entering Controller");
                var entradaResult = await _handler.HandleRegister(entradaDto);

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
        public async Task<IActionResult> GetAllOrderedBy(string sortBy, string sortOrder)
        {
            var providerResult = await _handler.HandleGetAll();
            //sortBy = FirstCharToUpper(sortBy);
            //var orderBy = typeof(Entrada).GetProperty(sortBy);

            //var orderedList = sortOrder == "desc" ? providerResult.OrderByDescending(x => orderBy.GetValue(x, null)).ToList() : providerResult.OrderBy(x => orderBy.GetValue(x, null)).ToList();

            return Ok(providerResult);
        }
    }
}