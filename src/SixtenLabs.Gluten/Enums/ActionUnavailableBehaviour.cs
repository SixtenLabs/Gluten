namespace SixtenLabs.Gluten
{
	/// <summary>
	/// What to do if the given target is null, or if the given action doesn't exist on the target
	/// </summary>
	public enum ActionUnavailableBehaviour
	{
		/// <summary>
		/// The default behaviour. What this is depends on whether this applies to an action or target, and an event or ICommand
		/// </summary>
		Default,

		/// <summary>
		/// Enable the control anyway. Clicking/etc the control won't do anything
		/// </summary>
		Enable,

		/// <summary>
		/// Disable the control. This is only valid for commands, not events
		/// </summary>
		Disable,

		/// <summary>
		/// An exception will be thrown when the control is clicked
		/// </summary>
		Throw
	}
}
