namespace SixtenLabs.Gluten
{
	/// <summary>
	/// Acts as a child. Knows about its parent
	/// </summary>
	public interface IChild
	{
		/// <summary>
		/// Gets or sets the parent object to this child
		/// </summary>
		object Parent { get; set; }
	}
}
