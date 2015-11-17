using System;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// EventArgs associated with the IScreenState.Activated event
	/// </summary>
	public class ActivationEventArgs : EventArgs
	{
		/// <summary>
		/// Gets a value indicating whether this is the first time this Screen has been activated, ever
		/// </summary>
		public bool IsInitialActivate { get; private set; }

		/// <summary>
		/// Gets the state being transitioned away from
		/// </summary>
		public ScreenState PreviousState { get; private set; }

		/// <summary>
		/// Initialises a new instance of the <see cref="ActivationEventArgs"/> class
		/// </summary>
		/// <param name="previousState">State being transitioned away from</param>
		/// <param name="isInitialActivate">True if this is the first time this screen has ever been activated</param>
		public ActivationEventArgs(ScreenState previousState, bool isInitialActivate)
		{
			IsInitialActivate = isInitialActivate;
			PreviousState = previousState;
		}
	}
}
