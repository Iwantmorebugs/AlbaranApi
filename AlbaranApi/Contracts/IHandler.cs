using System.Collections.Generic;
using System.Threading.Tasks;
using AlbaranApi.Dto;
using AlbaranApi.Models;

namespace AlbaranApi.Contracts
{
    public interface IHandler
    {
        Task<Entrada> HandleRegister(EntradaDto entradaDto);
        Task< IEnumerable<Entrada>> HandleGetAll();
    }
}