﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// Default implementation of IEventAggregator
	/// </summary>
	public class EventAggregator : IEventAggregator
	{
		/// <summary>
		/// Channel which handlers are subscribed to / messages are published to, if no channels are named
		/// </summary>
		public static readonly string DefaultChannel = "DefaultChannel";

		private readonly List<Handler> handlers = new List<Handler>();

		private readonly object handlersLock = new object();

		/// <summary>
		/// Register an instance as wanting to receive events. Implement IHandle{T} for each event type you want to receive.
		/// </summary>
		/// <param name="handler">Instance that will be registered with the EventAggregator</param>
		/// <param name="channels">Channel(s) which should be subscribed to. Defaults to EventAggregator.DefaultChannel if none given</param>
		public void Subscribe(IHandle handler, params string[] channels)
		{
			lock (handlersLock)
			{
				var isAlreadySubscribed = handlers.FirstOrDefault(x => x.IsHandlerForInstance(handler));

				if (isAlreadySubscribed == null)
				{
					// Adds default topic if appropriate
					handlers.Add(new Handler(handler, channels)); 
				}
				else
				{
					isAlreadySubscribed.SubscribeToChannels(channels);
				}
			}
		}

		/// <summary>
		/// Unregister as wanting to receive events. The instance will no longer receive events after this is called.
		/// </summary>
		/// <param name="handler">Instance to unregister</param>
		/// <param name="channels">Channel(s) to unsubscribe from. Unsubscribes from everything if no channels given</param>
		public void Unsubscribe(IHandle handler, params string[] channels)
		{
			lock (handlersLock)
			{
				var existingHandler = handlers.FirstOrDefault(x => x.IsHandlerForInstance(handler));

				if (existingHandler != null && existingHandler.UnsubscribeFromChannels(channels)) // Handles default topic appropriately
				{
					handlers.Remove(existingHandler);
				}
			}
		}

		/// <summary>
		/// Publish an event to all subscribers, using the specified dispatcher
		/// </summary>
		/// <param name="message">Event to publish</param>
		/// <param name="dispatcher">Dispatcher to use to call each subscriber's handle method(s)</param>
		/// <param name="channels">Channel(s) to publish the message to. Defaults to EventAggregator.DefaultChannel none given</param>
		public void PublishWithDispatcher(object message, Action<Action> dispatcher, params string[] channels)
		{
			lock (handlersLock)
			{
				var messageType = message.GetType();
				var deadHandlers = handlers.Where(x => !x.Handle(messageType, message, dispatcher, channels)).ToArray();

				foreach (var deadHandler in deadHandlers)
				{
					handlers.Remove(deadHandler);
				}
			}
		}

		private class Handler
		{
			private readonly WeakReference target;
			private readonly List<HandlerInvoker> invokers = new List<HandlerInvoker>();
			private readonly HashSet<string> channels = new HashSet<string>();

			public Handler(object handler, string[] channels)
			{
				var handlerType = handler.GetType();
				target = new WeakReference(handler);

				foreach (var implementation in handler.GetType().GetInterfaces().Where(x => x.IsGenericType && typeof(IHandle).IsAssignableFrom(x)))
				{
					var messageType = implementation.GetGenericArguments()[0];
					invokers.Add(new HandlerInvoker(handlerType, messageType, implementation.GetMethod("Handle")));
				}

				if (channels.Length == 0)
				{
					channels = new[] { DefaultChannel };
				}

				SubscribeToChannels(channels);
			}

			public bool IsHandlerForInstance(object subscriber)
			{
				return target.Target == subscriber;
			}

			public void SubscribeToChannels(string[] channels)
			{
				this.channels.UnionWith(channels);
			}

			public bool UnsubscribeFromChannels(string[] channels)
			{
				// If channels is empty, unsubscribe from everything
				if (channels.Length == 0)
				{
					return true;
				}

				this.channels.ExceptWith(channels);

				return this.channels.Count == 0;
			}

			public bool Handle(Type messageType, object message, Action<Action> dispatcher, string[] channels)
			{
				var target = this.target.Target;

				if (target == null)
				{
					return false;
				}

				if (channels.Length == 0)
				{
					channels = new[] { DefaultChannel };
				}

				// We're not subscribed to any of the channels
				if (!channels.All(x => this.channels.Contains(x)))
				{
					return true;
				}

				foreach (var invoker in invokers)
				{
					invoker.Invoke(target, messageType, message, dispatcher);
				}

				return true;
			}
		}

		private class HandlerInvoker
		{
			private readonly Type messageType;
			private readonly Action<object, object> invoker;

			public HandlerInvoker(Type targetType, Type messageType, MethodInfo invocationMethod)
			{
				this.messageType = messageType;
				var targetParam = Expression.Parameter(typeof(object), "target");
				var messageParam = Expression.Parameter(typeof(object), "message");
				var castTarget = Expression.Convert(targetParam, targetType);
				var castMessage = Expression.Convert(messageParam, messageType);
				var callExpression = Expression.Call(castTarget, invocationMethod, castMessage);
				invoker = Expression.Lambda<Action<object, object>>(callExpression, targetParam, messageParam).Compile();
			}

			public void Invoke(object target, Type messageType, object message, Action<Action> dispatcher)
			{
				if (this.messageType.IsAssignableFrom(messageType))
				{
					dispatcher(() => invoker(target, message));
				}
			}
		}
	}
}
