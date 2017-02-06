using DemoLightweightApi.Commands;
using MediatR;

namespace DemoLightweightApi.Handlers
{
    public class PingHandler : IRequestHandler<Ping, string>
    {
        public string Handle(Ping message)
        {
            return "Pong";
        }
    }
}