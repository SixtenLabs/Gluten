using System;
using System.Diagnostics.CodeAnalysis;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// Exception raise when the located View is of the wrong type (Window when expected UserControl, etc)
	/// </summary>
	[SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable")]
	public class StyletInvalidViewTypeException : Exception
	{
		/// <summary>
		/// Initialises a new instance of the <see cref="StyletInvalidViewTypeException"/> class
		/// </summary>
		/// <param name="message">Message associated with the Exception</param>
		public StyletInvalidViewTypeException(string message)
				: base(message)
		{ }
	}
}
