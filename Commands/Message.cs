using System;

namespace DemoLightweightApi.Commands
{
    public abstract class Message
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime Date {get;} = DateTime.Now;
    }
}