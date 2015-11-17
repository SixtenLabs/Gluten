using System;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// EventArgs associated with the IScreenState.Deactivated event
	/// </summary>
	public class DeactivationEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the state being transitioned away from
		/// </summary>
		public ScreenState PreviousState { get; private set; }

		/// <summary>
		/// Initialises a new instance of the <see cref="DeactivationEventArgs"/> class
		/// </summary>
		/// <param name="previousState">State being transitioned away from</param>
		public DeactivationEventArgs(ScreenState previousState)
		{
			PreviousState = previousState;
		}
	}
}
