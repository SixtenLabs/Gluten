using System.Threading.Tasks;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// Has an opinion on whether it should be closed
	/// </summary>
	/// <remarks>If implemented, CanCloseAsync should be called prior to closing the object</remarks>
	public interface IGuardClose
	{
		/// <summary>
		/// Returns whether or not the object can close, potentially asynchronously
		/// </summary>
		/// <returns>A task indicating whether the object can close</returns>
		Task<bool> CanCloseAsync();
	}
}
