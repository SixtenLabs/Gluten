using System.Collections.Generic;
using System.Windows;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// Manager capable of taking a ViewModel instance, instantiating its View and showing it as a dialog or window
	/// </summary>
	public interface IWindowManager
	{
		/// <summary>
		/// Given a ViewModel, show its corresponding View as a window
		/// </summary>
		/// <param name="viewModel">ViewModel to show the View for</param>
		void ShowWindow(object viewModel);

		/// <summary>
		/// Given a ViewModel, show its corresponding View as a Dialog
		/// </summary>
		/// <param name="viewModel">ViewModel to show the View for</param>
		/// <returns>DialogResult of the View</returns>
		bool? ShowDialog(object viewModel);

		/// <summary>
		/// Display a MessageBox
		/// </summary>
		/// <param name="messageBoxText">A System.String that specifies the text to display.</param>
		/// <param name="caption">A System.String that specifies the title bar caption to display.</param>
		/// <param name="buttons">A System.Windows.MessageBoxButton value that specifies which button or buttons to display.</param>
		/// <param name="icon">A System.Windows.MessageBoxImage value that specifies the icon to display.</param>
		/// <param name="defaultResult">A System.Windows.MessageBoxResult value that specifies the default result of the message box.</param>
		/// <param name="cancelResult">A System.Windows.MessageBoxResult value that specifies the cancel result of the message box</param>
		/// <param name="options">A System.Windows.MessageBoxOptions value object that specifies the options.</param>
		/// <param name="buttonLabels">A dictionary specifying the button labels, if desirable</param>
		/// <returns>The result chosen by the user</returns>
		MessageBoxResult ShowMessageBox(string messageBoxText, string caption = null,
				MessageBoxButton buttons = MessageBoxButton.OK,
				MessageBoxImage icon = MessageBoxImage.None,
				MessageBoxResult defaultResult = MessageBoxResult.None,
				MessageBoxResult cancelResult = MessageBoxResult.None,
				MessageBoxOptions options = MessageBoxOptions.None,
				IDictionary<MessageBoxResult, string> buttonLabels = null);
	}
}
