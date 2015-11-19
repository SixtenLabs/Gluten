using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// This will take the string name of a geometry resource and turn it into a format that will work as an ImageSource
	/// Brush defaults to Black if you do not pass one in.
	///  
	/// Example Geometry Resource:
	/// 
	/// <Geometry x:Key="Properties" x:Shared="false">
	/// F1 M 51.3993,40.6839L 55.3313, ...
	/// </Geometry>
	/// 
	/// Example Usage in Xaml:
	/// 
	/// This is a markupextension so you dont need to key it in the resources. Just use the reference to gluten and call it
	/// in the binding expression
	/// 
	/// Icon="{Binding AppIcon, Converter={gluten:GeometryToImageSourceConverter}}"
	/// 
	/// With Color Parameter (you will need to have your colors defined in a loaded resource dictionary
	/// Like so: <Color x:Key="IconSlateGray">SlateGray</Color>
	/// 
	/// Icon="{Binding AppIcon, Converter={codex:GeometryToImageSourceConverter}, ConverterParameter={StaticResource IconSlateGray}}"
	/// 
	/// </summary>
	public class GeometryToImageSourceConverter : ConverterMarkupExtension
	{
		protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var icon = Application.Current.FindResource(value);

			var brush = Brushes.Black;

			if (parameter != null && parameter is Color)
			{
				brush = new SolidColorBrush((Color)parameter);
			}

			var drawing = new GeometryDrawing(brush, null, icon as Geometry);
			var imageSource = new DrawingImage(drawing);

			return imageSource;
		}

		protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
