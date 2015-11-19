using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xaml;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// MarkupExtension used for binding Commands and Events to methods on the View.ActionTarget
	/// </summary>
	public class ActionExtension : MarkupExtension
	{
		/// <summary>
		/// Gets or sets the name of the method to call
		/// </summary>
		[ConstructorArgument("method")]
		public string Method { get; set; }

		/// <summary>
		/// Gets or sets the behaviour if the View.ActionTarget is nulil
		/// </summary>
		public ActionUnavailableBehaviour NullTarget { get; set; }

		/// <summary>
		/// Gets or sets the behaviour if the action itself isn't found on the View.ActionTarget
		/// </summary>
		public ActionUnavailableBehaviour ActionNotFound { get; set; }

		/// <summary>
		/// Initialises a new instance of the <see cref="ActionExtension"/> class
		/// </summary>
		public ActionExtension()
		{
		}

		/// <summary>
		/// Initialises a new instance of the <see cref="ActionExtension"/> class with the given method name
		/// </summary>
		/// <param name="method">Name of the method to call</param>
		public ActionExtension(string method)
		{
			Method = method;
		}

		private ActionUnavailableBehaviour CommandNullTargetBehaviour
		{
			get { return NullTarget == ActionUnavailableBehaviour.Default ? (Execute.InDesignMode ? ActionUnavailableBehaviour.Enable : ActionUnavailableBehaviour.Disable) : NullTarget; }
		}

		private ActionUnavailableBehaviour CommandActionNotFoundBehaviour
		{
			get { return ActionNotFound == ActionUnavailableBehaviour.Default ? ActionUnavailableBehaviour.Throw : ActionNotFound; }
		}

		private ActionUnavailableBehaviour EventNullTargetBehaviour
		{
			get { return NullTarget == ActionUnavailableBehaviour.Default ? ActionUnavailableBehaviour.Enable : NullTarget; }
		}

		private ActionUnavailableBehaviour EventActionNotFoundBehaviour
		{
			get { return ActionNotFound == ActionUnavailableBehaviour.Default ? ActionUnavailableBehaviour.Throw : ActionNotFound; }
		}

		/// <summary>
		/// When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension.
		/// </summary>
		/// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
		/// <returns>The object value to set on the property where the extension is applied.</returns>
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (Method == null)
			{
				throw new InvalidOperationException("Method has not been set");
			}

			var valueService = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));

			// Seems this is the case when we're in a template. We'll get called again properly in a second.
			// http://social.msdn.microsoft.com/Forums/vstudio/en-US/a9ead3d5-a4e4-4f9c-b507-b7a7d530c6a9/gaining-access-to-target-object-instead-of-shareddp-in-custom-markupextensions-providevalue-method?forum=wpf
			if (!(valueService.TargetObject is DependencyObject))
			{
				return this;
			}

			var targetObject = (DependencyObject)valueService.TargetObject;

			var rootObjectProvider = (IRootObjectProvider)serviceProvider.GetService(typeof(IRootObjectProvider));
			var rootObject = rootObjectProvider == null ? null : rootObjectProvider.RootObject as DependencyObject;

			var propertyAsDependencyProperty = valueService.TargetProperty as DependencyProperty;

			if (propertyAsDependencyProperty != null && propertyAsDependencyProperty.PropertyType == typeof(ICommand))
			{
				// If they're in design mode and haven't set View.ActionTarget, default to looking sensible
				return new CommandAction(targetObject, rootObject, Method, CommandNullTargetBehaviour, CommandActionNotFoundBehaviour);
			}

			var propertyAsEventInfo = valueService.TargetProperty as EventInfo;

			if (propertyAsEventInfo != null)
			{
				var ec = new EventAction(targetObject, rootObject, propertyAsEventInfo.EventHandlerType, Method, EventNullTargetBehaviour, EventActionNotFoundBehaviour);
				return ec.GetDelegate();
			}

			// For attached events
			var propertyAsMethodInfo = valueService.TargetProperty as MethodInfo;

			if (propertyAsMethodInfo != null)
			{
				var parameters = propertyAsMethodInfo.GetParameters();

				if (parameters.Length == 2 && typeof(Delegate).IsAssignableFrom(parameters[1].ParameterType))
				{
					var ec = new EventAction(targetObject, rootObject, parameters[1].ParameterType, Method, EventNullTargetBehaviour, EventActionNotFoundBehaviour);
					return ec.GetDelegate();
				}
			}

			throw new ArgumentException("Can only use ActionExtension with a Command property or an event handler");
		}
	}
}
