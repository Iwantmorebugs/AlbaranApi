﻿using System.Collections.Generic;
using AlbaranApi.Models;

namespace AlbaranApi.Contracts
{
    public interface IEntradaRepository
    {
        Entrada CreateEntry(Entrada entry);
        Entrada FindEntradaById(string entradaId);
        void Update(Entrada user);
        IEnumerable<Entrada> GetAllEntradas();
    }
}