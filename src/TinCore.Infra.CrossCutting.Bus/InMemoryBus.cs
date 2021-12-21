using System.Threading.Tasks;
using TinCore.Domain.Core.Bus;
using TinCore.Domain.Core.Commands;
using TinCore.Domain.Core.Events;
using MediatR;

namespace TinCore.Infra.CrossCutting.Bus
{
    public sealed class InMemoryBus : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public InMemoryBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public Task RaiseEvent<T>(T @event) where T : Event
        {            
            return _mediator.Publish(@event);
        }
    }
}