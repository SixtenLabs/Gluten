using System.Windows;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// Configuration passed to WindowManager (normally implemented by BootstrapperBase)
	/// </summary>
	public interface IWindowManagerConfig
	{
		/// <summary>
		/// Returns the currently-displayed window, or null if there is none (or it can't be determined)
		/// </summary>
		/// <returns>The currently-displayed window, or null</returns>
		Window GetActiveWindow();
	}
}
