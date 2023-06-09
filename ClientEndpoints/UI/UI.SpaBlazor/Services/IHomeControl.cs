using UI.SpaBlazor.Contracts.Commands;
using UI.SpaBlazor.Contracts.Events;

namespace UI.SpaBlazor.Services
{
    public interface IHomeControl
    {
        Task<R> SendAsync<C, R>(string uri, C command)
            where C : BaseHomeCommand
            where R : BaseHomeEvent;
    }
}
