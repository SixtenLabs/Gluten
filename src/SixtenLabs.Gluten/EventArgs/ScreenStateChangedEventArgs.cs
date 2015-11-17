using System;

namespace SixtenLabs.Gluten
{
	// <summary>
	/// EventArgs associated with the IScreenState.StateChanged event
	/// </summary>
	public class ScreenStateChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the state being transitioned to
		/// </summary>
		public ScreenState NewState { get; private set; }

		/// <summary>
		/// Gets the state being transitioned away from
		/// </summary>
		public ScreenState PreviousState { get; private set; }

		/// <summary>
		/// Initialises a new instance of the <see cref="ScreenStateChangedEventArgs"/> class
		/// </summary>
		/// <param name="newState">State being transitioned to</param>
		/// <param name="previousState">State being transitioned away from</param>
		public ScreenStateChangedEventArgs(ScreenState newState, ScreenState previousState)
		{
			NewState = newState;
			PreviousState = previousState;
		}
	}
}
