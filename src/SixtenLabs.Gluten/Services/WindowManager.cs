using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// Default implementation of IWindowManager, is capable of showing a ViewModel's View as a dialog or a window
	/// </summary>
	public class WindowManager : IWindowManager
	{
		private static readonly ILogger logger = LogManager.GetLogger(typeof(WindowManager));
		private readonly IViewManager viewManager;
		private readonly Func<IMessageBoxViewModel> messageBoxViewModelFactory;
		private readonly Func<Window> getActiveWindow;

		/// <summary>
		/// Initialises a new instance of the <see cref="WindowManager"/> class, using the given <see cref="IViewManager"/>
		/// </summary>
		/// <param name="viewManager">IViewManager to use when creating views</param>
		/// <param name="messageBoxViewModelFactory">Delegate which returns a new IMessageBoxViewModel instance when invoked</param>
		/// <param name="config">Configuration object</param>
		public WindowManager(IViewManager viewManager, Func<IMessageBoxViewModel> messageBoxViewModelFactory, IWindowManagerConfig config)
		{
			this.viewManager = viewManager;
			this.messageBoxViewModelFactory = messageBoxViewModelFactory;
			getActiveWindow = config.GetActiveWindow;
		}

		/// <summary>
		/// Given a ViewModel, show its corresponding View as a window
		/// </summary>
		/// <param name="viewModel">ViewModel to show the View for</param>
		public void ShowWindow(object viewModel)
		{
			CreateWindow(viewModel, false).Show();
		}

		/// <summary>
		/// Given a ViewModel, show its corresponding View as a Dialog
		/// </summary>
		/// <param name="viewModel">ViewModel to show the View for</param>
		/// <returns>DialogResult of the View</returns>
		public bool? ShowDialog(object viewModel)
		{
			return CreateWindow(viewModel, true).ShowDialog();
		}

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
		public MessageBoxResult ShowMessageBox(string messageBoxText, string caption = "",
				MessageBoxButton buttons = MessageBoxButton.OK,
				MessageBoxImage icon = MessageBoxImage.None,
				MessageBoxResult defaultResult = MessageBoxResult.None,
				MessageBoxResult cancelResult = MessageBoxResult.None,
				MessageBoxOptions options = MessageBoxOptions.None,
				IDictionary<MessageBoxResult, string> buttonLabels = null)
		{
			var vm = messageBoxViewModelFactory();
			vm.Setup(messageBoxText, caption, buttons, icon, defaultResult, cancelResult, options, buttonLabels);
			// Don't go through the IoC container to get the View. This means we can simplify it...
			var messageBoxView = new MessageBoxView();
			messageBoxView.InitializeComponent();
			viewManager.BindViewToModel(messageBoxView, vm);
			ShowDialog(vm);
			return vm.ClickedButton;
		}

		/// <summary>
		/// Given a ViewModel, create its View, ensure that it's a Window, and set it up
		/// </summary>
		/// <param name="viewModel">ViewModel to create the window for</param>
		/// <param name="isDialog">True if the window will be used as a dialog</param>
		/// <returns>Window which was created and set up</returns>
		protected virtual Window CreateWindow(object viewModel, bool isDialog)
		{
			var view = viewManager.CreateAndBindViewForModelIfNecessary(viewModel);
			var window = view as Window;
			if (window == null)
			{
				var e = new StyletInvalidViewTypeException(String.Format("WindowManager.ShowWindow or .ShowDialog tried to show a View of type '{0}', but that View doesn't derive from the Window class. " +
						"Make sure any Views you display using WindowManager.ShowWindow or .ShowDialog derive from Window (not UserControl, etc)",
						view == null ? "(null)" : view.GetType().Name));
				logger.Error(e);
				throw e;
			}

			// Only set this it hasn't been set / bound to anything
			var haveDisplayName = viewModel as IHaveDisplayName;
			if (haveDisplayName != null && (String.IsNullOrEmpty(window.Title) || window.Title == view.GetType().Name) && BindingOperations.GetBindingBase(window, Window.TitleProperty) == null)
			{
				var binding = new Binding("DisplayName") { Mode = BindingMode.TwoWay };
				window.SetBinding(Window.TitleProperty, binding);
			}

			if (isDialog)
			{
				var owner = InferOwnerOf(window);
				if (owner != null)
				{
					// We can end up in a really weird situation if they try and display more than one dialog as the application's closing
					// Basically the MainWindow's no long active, so the second dialog chooses the first dialog as its owner... But the first dialog
					// hasn't yet been shown, so we get an exception ("cannot set owner property to a Window which has not been previously shown").
					try
					{
						window.Owner = owner;
					}
					catch (InvalidOperationException e)
					{
						logger.Error(e, "This can occur when the application is closing down");
					}
				}

				logger.Info("Displaying ViewModel {0} with View {1} as a Dialog", viewModel, window);
			}
			else
			{
				logger.Info("Displaying ViewModel {0} with View {1} as a Window", viewModel, window);
			}

			// If and only if they haven't tried to position the window themselves...
			// Has to be done after we're attempted to set the owner
			if (window.WindowStartupLocation == WindowStartupLocation.Manual && Double.IsNaN(window.Top) && Double.IsNaN(window.Left))
				window.WindowStartupLocation = window.Owner == null ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;

			// This gets itself retained by the window, by registering events
			// ReSharper disable once ObjectCreationAsStatement
			new WindowConductor(window, viewModel);

			return window;
		}

		private Window InferOwnerOf(Window window)
		{
			var active = getActiveWindow();
			return ReferenceEquals(active, window) ? null : active;
		}

		private class WindowConductor : IChildDelegate
		{
			private readonly Window window;
			private readonly object viewModel;

			public WindowConductor(Window window, object viewModel)
			{
				this.window = window;
				this.viewModel = viewModel;

				// They won't be able to request a close unless they implement IChild anyway...
				var viewModelAsChild = this.viewModel as IChild;
				if (viewModelAsChild != null)
					viewModelAsChild.Parent = this;

				ScreenExtensions.TryActivate(this.viewModel);

				var viewModelAsScreenState = this.viewModel as IScreenState;
				if (viewModelAsScreenState != null)
				{
					window.StateChanged += WindowStateChanged;
					window.Closed += WindowClosed;
				}

				if (this.viewModel is IGuardClose)
					window.Closing += WindowClosing;
			}

			private void WindowStateChanged(object sender, EventArgs e)
			{
				switch (window.WindowState)
				{
					case WindowState.Maximized:
					case WindowState.Normal:
						logger.Info("Window {0} maximized/restored: activating", window);
						ScreenExtensions.TryActivate(viewModel);
						break;

					case WindowState.Minimized:
						logger.Info("Window {0} minimized: deactivating", window);
						ScreenExtensions.TryDeactivate(viewModel);
						break;
				}
			}

			private void WindowClosed(object sender, EventArgs e)
			{
				// Logging was done in the Closing handler

				window.StateChanged -= WindowStateChanged;
				window.Closed -= WindowClosed;
				window.Closing -= WindowClosing; // Not sure this is required

				ScreenExtensions.TryClose(viewModel);
			}

			private async void WindowClosing(object sender, CancelEventArgs e)
			{
				if (e.Cancel)
					return;

				logger.Info("ViewModel {0} close requested because its View was closed", viewModel);

				// See if the task completed synchronously
				var task = ((IGuardClose)viewModel).CanCloseAsync();
				if (task.IsCompleted)
				{
					// The closed event handler will take things from here if we don't cancel
					if (!task.Result)
						logger.Info("Close of ViewModel {0} cancelled because CanCloseAsync returned false", viewModel);
					e.Cancel = !task.Result;
				}
				else
				{
					e.Cancel = true;
					logger.Info("Delaying closing of ViewModel {0} because CanCloseAsync is completing asynchronously", viewModel);
					if (await task)
					{
						window.Closing -= WindowClosing;
						window.Close();
						// The Closed event handler handles unregistering the events, and closing the ViewModel
					}
					else
					{
						logger.Info("Close of ViewModel {0} cancelled because CanCloseAsync returned false", viewModel);
					}
				}
			}

			/// <summary>
			/// Close was requested by the child
			/// </summary>
			/// <param name="item">Item to close</param>
			/// <param name="dialogResult">DialogResult to close with, if it's a dialog</param>
			async void IChildDelegate.CloseItem(object item, bool? dialogResult)
			{
				if (item != viewModel)
				{
					logger.Warn("IChildDelegate.CloseItem called with item {0} which is _not_ our ViewModel {1}", item, viewModel);
					return;
				}

				var guardClose = viewModel as IGuardClose;
				if (guardClose != null && !await guardClose.CanCloseAsync())
				{
					logger.Info("Close of ViewModel {0} cancelled because CanCloseAsync returned false", viewModel);
					return;
				}

				logger.Info("ViewModel {0} close requested with DialogResult {1} because it called RequestClose", viewModel, dialogResult);

				window.StateChanged -= WindowStateChanged;
				window.Closed -= WindowClosed;
				window.Closing -= WindowClosing;

				// Need to call this after unregistering the event handlers, as it causes the window
				// to be closed
				if (dialogResult != null)
					window.DialogResult = dialogResult;

				ScreenExtensions.TryClose(viewModel);

				window.Close();
			}
		}
	}
}
