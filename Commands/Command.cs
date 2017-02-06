using MediatR;

namespace DemoLightweightApi.Commands
{
    public abstract class Command : Message, IRequest<object>
    {
    }
}