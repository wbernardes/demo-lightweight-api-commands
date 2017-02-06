using DemoLightweightApi.Commands;
using MediatR;

namespace DemoLightweightApi.Handlers
{
    public class PingHandler : IRequestHandler<Ping, object>
    {
        public object Handle(Ping message)
        {
            return "Pong";
        }
    }
}