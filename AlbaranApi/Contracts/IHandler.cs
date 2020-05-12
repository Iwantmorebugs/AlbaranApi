using System.Collections.Generic;
using AlbaranApi.Dto;
using AlbaranApi.Models;

namespace AlbaranApi.Contracts
{
    public interface IHandler
    {
        Entrada HandleRegister(EntradaDto entradaDto);
        IEnumerable<Entrada> HandleGetAll();
    }
}