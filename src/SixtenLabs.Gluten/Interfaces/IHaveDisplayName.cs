namespace SixtenLabs.Gluten
{
	/// <summary>
	/// Has a display name. In reality, this is bound to things like Window titles and TabControl tabs
	/// </summary>
	public interface IHaveDisplayName
	{
		/// <summary>
		/// Gets or sets the name which should be displayed
		/// </summary>
		string DisplayName { get; set; }
	}
}
