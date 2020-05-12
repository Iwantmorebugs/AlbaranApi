using System;
using System.Threading.Tasks;
using AlbaranApi.Contracts;
using Inventario.EventResult.Contracts;
using MassTransit;

namespace AlbaranApi.Publisher
{
    public class PublishDomainEventResultConsumer : IDomainEventResultPublisher
    {
        private readonly IBus _messageBus;

        public PublishDomainEventResultConsumer(IBus messageBus)
        {
            _messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
        }

        public async Task Consume(ICommandResultDto eventResult)
        {
            try
            {
                await _messageBus.Publish(eventResult);
            }
            catch (Exception exception)
            {
                var trace =
                    new
                    {
                        Time = DateTime.Now,
                        Message = "Can't publish domain event",
                        Exception = exception,
                        BusinessProcessId = eventResult
                    };

                Console.WriteLine(trace);
            }
        }
    }
}