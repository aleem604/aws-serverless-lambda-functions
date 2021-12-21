using System.Threading.Tasks;
using TinCore.Domain.Core.Commands;
using TinCore.Domain.Core.Events;


namespace TinCore.Domain.Core.Bus
{
    public interface IMediatorHandler
    {
        Task SendCommand<T>(T command) where T : Command;
        Task RaiseEvent<T>(T @event) where T : Event;
    }
}
