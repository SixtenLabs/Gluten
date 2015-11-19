namespace SixtenLabs.Gluten
{
	/// <summary>
	/// Extension methods on IEventAggregator, to give more dispatching options
	/// </summary>
	public static class EventAggregatorExtensions
	{
		/// <summary>
		/// Publish an event to all subscribers, calling the handle methods on the UI thread
		/// </summary>
		/// <param name="eventAggregator">EventAggregator to publish the message with</param>
		/// <param name="message">Event to publish</param>
		/// <param name="channels">Channel(s) to publish the message to. Defaults to EventAggregator.DefaultChannel none given</param>
		public static void PublishOnUIThread(this IEventAggregator eventAggregator, object message, params string[] channels)
		{
			eventAggregator.PublishWithDispatcher(message, Execute.OnUIThread, channels);
		}

		/// <summary>
		/// Publish an event to all subscribers, calling the handle methods synchronously on the current thread
		/// </summary>
		/// <param name="eventAggregator">EventAggregator to publish the message with</param>
		/// <param name="message">Event to publish</param>
		/// <param name="channels">Channel(s) to publish the message to. Defaults to EventAggregator.DefaultChannel none given</param>
		public static void Publish(this IEventAggregator eventAggregator, object message, params string[] channels)
		{
			eventAggregator.PublishWithDispatcher(message, a => a(), channels);
		}
	}
}
