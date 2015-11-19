using System;
using System.Diagnostics.CodeAnalysis;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// The method specified does not have the correct signature
	/// </summary>
	[SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable")]
	public class ActionSignatureInvalidException : Exception
	{
		internal ActionSignatureInvalidException(string message) : base(message) { }
	}
}
