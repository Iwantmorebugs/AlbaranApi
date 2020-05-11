using AlbaranApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AlbaranApi.Contracts
{
    public interface IEntradaContext
    {
        DbSet<Entrada> Entradas { get; set; }
        int SaveChanges(); //need not to implement explicitly as it is implemented inside DBContext
    }
}