using System;
using System.Diagnostics.CodeAnalysis;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// Exception raised while attempting to locate a View for a ViewModel
	/// </summary>
	[SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable")]
	public class StyletViewLocationException : Exception
	{
		/// <summary>
		/// Name of the View in question
		/// </summary>
		public readonly string ViewTypeName;

		/// <summary>
		/// Initialises a new instance of the <see cref="StyletViewLocationException"/> class
		/// </summary>
		/// <param name="message">Message associated with the Exception</param>
		/// <param name="viewTypeName">Name of the View this question was thrown for</param>
		public StyletViewLocationException(string message, string viewTypeName)
				: base(message)
		{
			ViewTypeName = viewTypeName;
		}
	}
}
