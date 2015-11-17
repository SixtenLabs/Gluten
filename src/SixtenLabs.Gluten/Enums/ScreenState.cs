namespace SixtenLabs.Gluten
{
	/// <summary>
	/// State in which a screen can be
	/// </summary>
	public enum ScreenState
	{
		/// <summary>
		/// Screen is active. It is likely being displayed to the user
		/// </summary>
		Active,

		/// <summary>
		/// Screen is deactivated. It is either new, has been hidden in favour of another Screen, or the entire window has been minimised
		/// </summary>
		Deactivated,

		/// <summary>
		/// Screen has been closed. It has no associated View, but may yet be displayed again
		/// </summary>
		Closed,
	}
}
