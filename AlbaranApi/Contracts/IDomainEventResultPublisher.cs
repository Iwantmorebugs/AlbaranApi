using System.Threading.Tasks;
using Inventario.EventResult.Contracts;

namespace AlbaranApi.Contracts
{
    public interface IDomainEventResultPublisher
    {
        Task Consume(ICommandResultAlbaranDto eventResult);
    }
}