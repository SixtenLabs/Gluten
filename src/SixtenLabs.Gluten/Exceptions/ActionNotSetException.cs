using System;
using System.Diagnostics.CodeAnalysis;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// The View.ActionTarget was not set. This probably means the item is in a ContextMenu/Popup
	/// </summary>
	[SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable")]
	public class ActionNotSetException : Exception
	{
		internal ActionNotSetException(string message) : base(message) { }
	}
}
