using System;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// Has a concept of state, which can be manipulated by its Conductor
	/// </summary>
	public interface IScreenState
	{
		/// <summary>
		/// Gets the current state of the Screen
		/// </summary>
		ScreenState State { get; }

		/// <summary>
		/// Gets a value indicating whether the current state is ScreenState.Active
		/// </summary>
		bool IsActive { get; }

		/// <summary>
		/// Raised when the Screen's state changed, for any reason
		/// </summary>
		event EventHandler<ScreenStateChangedEventArgs> StateChanged;

		/// <summary>
		/// Raised when the object is actually activated
		/// </summary>
		event EventHandler<ActivationEventArgs> Activated;

		/// <summary>
		/// Raised when the object is actually deactivated
		/// </summary>
		event EventHandler<DeactivationEventArgs> Deactivated;

		/// <summary>
		/// Raised when the object is actually closed
		/// </summary>
		event EventHandler<CloseEventArgs> Closed;

		/// <summary>
		/// Activate the object. May not actually cause activation (e.g. if it's already active)
		/// </summary>
		void Activate();

		/// <summary>
		/// Deactivate the object. May not actually cause deactivation (e.g. if it's already deactive)
		/// </summary>
		void Deactivate();

		/// <summary>
		/// Close the object. May not actually cause closure (e.g. if it's already closed)
		/// </summary>
		void Close();
	}
}
