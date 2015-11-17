using System;
using System.Windows;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// Added to your App.xaml, this is responsible for loading the Boostrapper you specify, and Stylet's other resources
	/// </summary>
	public class ApplicationLoader : ResourceDictionary
	{
		private readonly ResourceDictionary resourceDictionary;

		private const string DefaultResourceDictionaryPath = "pack://application:,,,/SixtenLabs.Gluten;component/Xaml/GlutenResourceDictionary.xaml";

		/// <summary>
		/// Initialises a new instance of the <see cref="ApplicationLoader"/> class
		/// </summary>
		public ApplicationLoader()
		{
			resourceDictionary = new ResourceDictionary() { Source = new Uri(DefaultResourceDictionaryPath, UriKind.Absolute) };
			LoadResources = true;
		}

		private IBootstrapper bootstrapper;

		/// <summary>
		/// Gets or sets the bootstrapper instance to use to start your application. This must be set.
		/// </summary>
		public IBootstrapper Bootstrapper
		{
			get { return bootstrapper; }
			set
			{
				bootstrapper = value;
				bootstrapper.Setup(Application.Current);
			}
		}

		private bool loadResources;

		/// <summary>
		/// Gets or sets a value indicating whether to load Stylet's own resources (e.g. StyletConductorTabControl). Defaults to true.
		/// </summary>
		public bool LoadResources
		{
			get { return loadResources; }
			set
			{
				loadResources = value;

				if (loadResources)
				{
					MergedDictionaries.Add(resourceDictionary);
				}
				else
				{
					MergedDictionaries.Remove(resourceDictionary);
				}
			}
		}
	}
}
