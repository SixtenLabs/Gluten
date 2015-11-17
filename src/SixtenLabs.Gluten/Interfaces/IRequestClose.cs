namespace SixtenLabs.Gluten
{
	/// <summary>
	/// Get the object to request that its parent close it
	/// </summary>
	public interface IRequestClose
	{
		/// <summary>
		/// Request that the conductor responsible for this screen close it
		/// </summary>
		/// <param name="dialogResult">DialogResult to return, if this is a dialog</param>
		void RequestClose(bool? dialogResult = null);
	}
}
