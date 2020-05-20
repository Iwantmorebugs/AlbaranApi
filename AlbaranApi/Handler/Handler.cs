using AlbaranApi.Contracts;
using AlbaranApi.Dto;
using AlbaranApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlbaranApi.Handler
{
    public class Handler : IHandler
    {
        private readonly IDomainEventResultPublisher _domainEventResultPublisher;
        private readonly IEntradaRepository _entradaRepository;
        private readonly IAlbaranQrService _albaranQrService;

        public Handler(IEntradaRepository entradaRepository, IAlbaranQrService albaranQrService,
            IDomainEventResultPublisher domainEventResultPublisher)
        {
            _entradaRepository = entradaRepository;
            _albaranQrService = albaranQrService;
            _domainEventResultPublisher = domainEventResultPublisher;
        }

        public async Task<Entrada> HandleRegister(EntradaDto entradaDto)
        {
            HandleRegisterTrace();

            var entrada = _albaranQrService.HandleAlbaranQrData(entradaDto);
            entrada= _albaranQrService.HandleProductQrData(entrada);

            await _entradaRepository.CreateEntry(entrada);

            var resultToBePublished = _albaranQrService.CreatePublishableResult(entrada);

            if (entrada != null) await _domainEventResultPublisher.Consume(resultToBePublished);

            return entrada;
        }

        public async Task<IEnumerable<Entrada>> HandleGetAll()
        {
            var result = await _entradaRepository.GetAllEntradas();
            return result;
        }

        private static void HandleRegisterTrace()
        {
            var trace =
                new
                {
                    Time = DateTime.Now,
                    Message = "Entering Handler"
                };
            Console.WriteLine(trace);
        }

    }
}