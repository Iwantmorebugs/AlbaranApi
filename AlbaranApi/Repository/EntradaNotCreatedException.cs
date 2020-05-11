using System;

namespace AlbaranApi.Repository
{
    public class EntradaNotCreatedException : ApplicationException
    {
        public EntradaNotCreatedException()
        {
        }
        public EntradaNotCreatedException(string entradaId): base($"Entrada with this id {entradaId} already exists")
        {
        }
    }
}
