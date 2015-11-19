using System;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// Centralised, weakly-binding publish/subscribe event manager
	/// </summary>
	public interface IEventAggregator
	{
		/// <summary>
		/// Register an instance as wanting to receive events. Implement IHandle{T} for each event type you want to receive.
		/// </summary>
		/// <param name="handler">Instance that will be registered with the EventAggregator</param>
		/// <param name="channels">Channel(s) which should be subscribed to. Defaults to EventAggregator.DefaultChannel if none given</param>
		void Subscribe(IHandle handler, params string[] channels);

		/// <summary>
		/// Unregister as wanting to receive events. The instance will no longer receive events after this is called.
		/// </summary>
		/// <param name="handler">Instance to unregister</param>
		/// <param name="channels">Channel(s) to unsubscribe from. Unsubscribes from everything if no channels given</param>
		void Unsubscribe(IHandle handler, params string[] channels);

		/// <summary>
		/// Publish an event to all subscribers, using the specified dispatcher
		/// </summary>
		/// <param name="message">Event to publish</param>
		/// <param name="dispatcher">Dispatcher to use to call each subscriber's handle method(s)</param>
		/// <param name="channels">Channel(s) to publish the message to. Defaults to EventAggregator.DefaultChannel none given</param>
		void PublishWithDispatcher(object message, Action<Action> dispatcher, params string[] channels);
	}
}
