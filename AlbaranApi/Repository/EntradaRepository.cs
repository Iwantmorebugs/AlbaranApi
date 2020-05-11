using System;
using System.Collections.Generic;
using System.Linq;
using AlbaranApi.Contracts;
using AlbaranApi.Models;

namespace AlbaranApi.Repository
{
    public class EntradaRepository : IEntradaRepository
    {
        private readonly IEntradaContext _context;
        public EntradaRepository(IEntradaContext context)
        {
            _context = context;
        }
        public Entrada CreateEntry(Entrada entrada)
        {
            if (_context.Entradas.Any())
                if (_context.Entradas.Any(x => x.EntradaId == entrada.EntradaId))
                    throw new Exception("Entrada \"" + entrada.EntradaId + "\" is already taken");

            
            _context.Entradas.Add(entrada);
            _context.SaveChanges();
            return entrada;
        }
        public Entrada FindEntradaById(string entradaId)
        {
            return _context.Entradas.FirstOrDefault(x => x.EntradaId.Equals(entradaId));
        }
        public void Update(Entrada entrada)
        {
            try
            {
                _context.Entradas.Update(entrada);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public IEnumerable<Entrada> GetAllEntradas()
        {
            return _context.Entradas.Select(x => x);
        }
    }
}