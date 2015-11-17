using System.Collections.Generic;
using System.Collections.Specialized;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// Interface encapsulating IReadOnlyList and INotifyCollectionChanged
	/// </summary>
	/// <typeparam name="T">The type of elements in the collection</typeparam>
	public interface IReadOnlyObservableCollection<out T> : IReadOnlyList<T>, INotifyCollectionChanged, INotifyCollectionChanging
	{
	}
}
