using System;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// EventArgs associated with the IScreenState.Closed event
	/// </summary>
	public class CloseEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the state being transitioned away from
		/// </summary>
		public ScreenState PreviousState { get; private set; }

		/// <summary>
		/// Initialises a new instance of the <see cref="CloseEventArgs"/> class
		/// </summary>
		/// <param name="previousState">State being transitioned away from</param>
		public CloseEventArgs(ScreenState previousState)
		{
			PreviousState = previousState;
		}
	}
}
