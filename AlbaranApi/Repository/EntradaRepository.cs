using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AlbaranApi.Contracts;
using AlbaranApi.Models;
using MongoDB.Driver;

namespace AlbaranApi.Repository
{
    public class EntradaRepository : IEntradaRepository

    {
        private readonly IMongoCollection<Entrada> _entradaRepository;


        public EntradaRepository(IMongoDatabaseFactory mongoDatabase)
        {
            if (mongoDatabase is null) throw new ArgumentNullException(nameof(mongoDatabase));

            var database = mongoDatabase.Create();
            _entradaRepository = database.GetCollection<Entrada>("Albaranes");
        }

        public async Task CreateEntry(Entrada entry)
        {
            await _entradaRepository.InsertOneAsync(entry);
        }

        public async Task<Entrada>  FindEntradaById(string entradaId)
        {
            var queryResult = await _entradaRepository.FindAsync(x => x.EntradaId.Equals(entradaId));

            var existenciasHistory = await queryResult.SingleOrDefaultAsync();

            return existenciasHistory;
        }

        public async Task Update(Entrada entrada)
        {
            var update =
                Builders<Entrada>
                    .Update
                    .Set(x => x.EntradaProductos, entrada.EntradaProductos)
                    .Set(x => x.ProviderId, entrada.ProviderId);

            Expression<Func<Entrada, bool>> expression = x => x.EntradaId == entrada.EntradaId;
            await
                _entradaRepository
                    .UpdateOneAsync(expression, update);
        }

        public Task<IEnumerable<Entrada>> GetAllEntradas()
        {
            throw new NotImplementedException();
        }
    }
}