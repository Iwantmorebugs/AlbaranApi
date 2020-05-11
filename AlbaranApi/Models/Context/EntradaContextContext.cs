using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlbaranApi.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AlbaranApi.Models.Context
{
    public class EntradaContextContext: DbContext, IEntradaContext
    {
        public DbSet<Entrada> Entradas { get; set; }

        public EntradaContextContext(DbContextOptions<EntradaContextContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
