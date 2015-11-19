using System.Collections.Generic;
using System.Windows;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// Interface for a MessageBoxViewModel. MessageBoxWindowManagerExtensions.ShowMessageBox will use the configured implementation of this
	/// </summary>
	public interface IMessageBoxViewModel
	{
		/// <summary>
		/// Setup the MessageBoxViewModel with the information it needs
		/// </summary>
		/// <param name="messageBoxText">A System.String that specifies the text to display.</param>
		/// <param name="caption">A System.String that specifies the title bar caption to display.</param>
		/// <param name="buttons">A System.Windows.MessageBoxButton value that specifies which button or buttons to display.</param>
		/// <param name="icon">A System.Windows.MessageBoxImage value that specifies the icon to display.</param>
		/// <param name="defaultResult">A System.Windows.MessageBoxResult value that specifies the default result of the message box.</param>
		/// <param name="cancelResult">A System.Windows.MessageBoxResult value that specifies the cancel result of the message box</param>
		/// <param name="options">A System.Windows.MessageBoxOptions value object that specifies the options.</param>
		/// <param name="buttonLabels">A dictionary specifying the button labels, if desirable</param>
		void Setup(
				string messageBoxText,
				string caption = null,
				MessageBoxButton buttons = MessageBoxButton.OK,
				MessageBoxImage icon = MessageBoxImage.None,
				MessageBoxResult defaultResult = MessageBoxResult.None,
				MessageBoxResult cancelResult = MessageBoxResult.None,
				MessageBoxOptions options = MessageBoxOptions.None,
				IDictionary<MessageBoxResult, string> buttonLabels = null);

		/// <summary>
		/// Gets the button clicked by the user, after they've clicked it
		/// </summary>
		MessageBoxResult ClickedButton { get; }
	}
}
