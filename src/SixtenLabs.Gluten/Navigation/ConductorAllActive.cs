using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace SixtenLabs.Gluten
{
	public partial class Conductor<T>
	{
		/// <summary>
		/// Contains specific Conductor{T} collection types
		/// </summary>
		public partial class Collection
		{
			/// <summary>
			/// Conductor which has many items, all of which active at the same time
			/// </summary>
			public class AllActive : ConductorBase<T>
			{
				private readonly BindableCollection<T> items = new BindableCollection<T>();

				private T[] itemsBeforeReset;

				/// <summary>
				/// Gets all items associated with this conductor
				/// </summary>
				public IObservableCollection<T> Items
				{
					get { return items; }
				}

				/// <summary>
				/// Initialises a new instance of the <see cref="Conductor{T}.Collection.AllActive"/> class
				/// </summary>
				public AllActive()
				{
					items.CollectionChanging += (o, e) =>
										{
											switch (e.Action)
											{
												case NotifyCollectionChangedAction.Reset:
													itemsBeforeReset = items.ToArray();
													break;
											}
										};

					items.CollectionChanged += (o, e) =>
										{
											switch (e.Action)
											{
												case NotifyCollectionChangedAction.Add:
													ActivateAndSetParent(e.NewItems);
													break;

												case NotifyCollectionChangedAction.Remove:
													this.CloseAndCleanUp(e.OldItems, DisposeChildren);
													break;

												case NotifyCollectionChangedAction.Replace:
													ActivateAndSetParent(e.NewItems);
													this.CloseAndCleanUp(e.OldItems, DisposeChildren);
													break;

												case NotifyCollectionChangedAction.Reset:
													ActivateAndSetParent(items.Except(itemsBeforeReset));
													this.CloseAndCleanUp(itemsBeforeReset.Except(items), DisposeChildren);
													itemsBeforeReset = null;
													break;
											}
										};
				}

				/// <summary>
				/// Active all items in a given collection if appropriate, and set the parent of all items to this
				/// </summary>
				/// <param name="items">Items to manipulate</param>
				protected virtual void ActivateAndSetParent(IEnumerable items)
				{
					this.SetParentAndSetActive(items, IsActive);
				}

				/// <summary>
				/// Activates all items whenever this conductor is activated
				/// </summary>
				protected override void OnActivate()
				{
					foreach (var item in items.OfType<IScreenState>())
					{
						item.Activate();
					}
				}

				/// <summary>
				/// Deactivates all items whenever this conductor is deactivated
				/// </summary>
				protected override void OnDeactivate()
				{
					foreach (var item in items.OfType<IScreenState>())
					{
						item.Deactivate();
					}
				}

				/// <summary>
				/// Close, and clean up, all items when this conductor is closed
				/// </summary>
				protected override void OnClose()
				{
					// We've already been deactivated by this point    
					foreach (var item in items)
					{
						this.CloseAndCleanUp(item, DisposeChildren);
					}

					items.Clear();
				}

				/// <summary>
				/// Determine if the conductor can close. Returns true if and when all items can close
				/// </summary>
				/// <returns>A Task indicating whether this conductor can close</returns>
				public override Task<bool> CanCloseAsync()
				{
					return CanAllItemsCloseAsync(items);
				}

				/// <summary>
				/// Activate the given item, and add it to the Items collection
				/// </summary>
				/// <param name="item">Item to activate</param>
				public override void ActivateItem(T item)
				{
					if (item == null)
						return;

					EnsureItem(item);

					if (IsActive)
						ScreenExtensions.TryActivate(item);
					else
						ScreenExtensions.TryDeactivate(item);
				}

				/// <summary>
				/// Deactive the given item
				/// </summary>
				/// <param name="item">Item to deactivate</param>
				public override void DeactivateItem(T item)
				{
					ScreenExtensions.TryDeactivate(item);
				}

				/// <summary>
				/// Close a particular item, removing it from the Items collection
				/// </summary>
				/// <param name="item">Item to close</param>
				public async override void CloseItem(T item)
				{
					if (item == null)
						return;

					if (await CanCloseItem(item))
					{
						this.CloseAndCleanUp(item, DisposeChildren);
						items.Remove(item);
					}
				}

				/// <summary>
				/// Returns all children of this parent
				/// </summary>
				/// <returns>All children associated with this conductor</returns>
				public override IEnumerable<T> GetChildren()
				{
					return items;
				}

				/// <summary>
				/// Ensure an item is ready to be activated, by adding it to the items collection, as well as setting it up
				/// </summary>
				/// <param name="newItem">Item to ensure</param>
				protected override void EnsureItem(T newItem)
				{
					if (!items.Contains(newItem))
						items.Add(newItem);

					base.EnsureItem(newItem);
				}
			}
		}
	}
}
