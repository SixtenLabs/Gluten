using System.Windows;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// Responsible for managing views. Locates the correct view, instantiates it, attaches it to its ViewModel correctly, and handles the View.Model attached property
	/// </summary>
	public interface IViewManager
	{
		/// <summary>
		/// Called by View whenever its current View.Model changes. Will locate and instantiate the correct view, and set it as the target's Content
		/// </summary>
		/// <param name="targetLocation">Thing which View.Model was changed on. Will have its Content set</param>
		/// <param name="oldValue">Previous value of View.Model</param>
		/// <param name="newValue">New value of View.Model</param>
		void OnModelChanged(DependencyObject targetLocation, object oldValue, object newValue);

		/// <summary>
		/// Given a ViewModel instance, locate its View type (using LocateViewForModel), and instantiates it
		/// </summary>
		/// <param name="model">ViewModel to locate and instantiate the View for</param>
		/// <returns>Instantiated and setup view</returns>
		UIElement CreateViewForModel(object model);

		/// <summary>
		/// Given an instance of a ViewModel and an instance of its View, bind the two together
		/// </summary>
		/// <param name="view">View to bind to the ViewModel</param>
		/// <param name="viewModel">ViewModel to bind the View to</param>
		void BindViewToModel(UIElement view, object viewModel);

		/// <summary>
		/// Create a View for the given ViewModel, and bind the two together, if the model doesn't already have a view
		/// </summary>
		/// <param name="model">ViewModel to create a Veiw for</param>
		/// <returns>Newly created View, bound to the given ViewModel</returns>
		UIElement CreateAndBindViewForModelIfNecessary(object model);
	}
}
