using System.Collections.Generic;
using System.Threading.Tasks;
using AlbaranApi.Models;

namespace AlbaranApi.Contracts
{
    public interface IEntradaRepository
    {
        Task CreateEntry(Entrada entry);
        Task<Entrada>  FindEntradaById(string entradaId);
        Task Update(Entrada user);
        Task<IEnumerable<Entrada>> GetAllEntradas();
    }
}