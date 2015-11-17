using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// Represents a collection which is observasble
	/// </summary>
	/// <typeparam name="T">The type of elements in the collections</typeparam>
	public interface IObservableCollection<T> : IList<T>, INotifyPropertyChanged, INotifyCollectionChanged
	{
		/// <summary>
		/// Add a range of items
		/// </summary>
		/// <param name="items">Items to add</param>
		void AddRange(IEnumerable<T> items);

		/// <summary>
		/// Remove a range of items
		/// </summary>
		/// <param name="items">Items to remove</param>
		void RemoveRange(IEnumerable<T> items);
	}
}
