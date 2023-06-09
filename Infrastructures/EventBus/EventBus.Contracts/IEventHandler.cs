namespace EventBus.Contracts
{
	public interface IEventHandler<TEvent> where TEvent : IEvent
	{
		Task Handle(IEvent @event);
	}
}