using System;
using System.Diagnostics.CodeAnalysis;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// The method specified could not be found on the Action Target
	/// </summary>
	[SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable")]
	public class ActionNotFoundException : Exception
	{
		internal ActionNotFoundException(string message) : base(message) { }
	}
}
