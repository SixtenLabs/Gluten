using System;
using System.Diagnostics.CodeAnalysis;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// The Action Target was null, and shouldn't have been (NullTarget = Throw)
	/// </summary>
	[SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable")]
	public class ActionTargetNullException : Exception
	{
		internal ActionTargetNullException(string message) : base(message) { }
	}
}
