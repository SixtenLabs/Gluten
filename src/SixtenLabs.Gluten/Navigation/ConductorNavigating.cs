using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SixtenLabs.Gluten
{
	public partial class Conductor<T>
	{
		/// <summary>
		/// Stack-based navigation. A Conductor which has one active item, and a stack of previous items
		/// </summary>
		public class StackNavigation : ConductorBaseWithActiveItem<T>
		{
			// We need to remove arbitrary items, so no Stack<T> here!
			private readonly List<T> history = new List<T>();

			/// <summary>
			/// Activate the given item. This deactivates the previous item, and pushes it onto the history stack
			/// </summary>
			/// <param name="item">Item to activate</param>
			public override void ActivateItem(T item)
			{
				if (item != null && item.Equals(ActiveItem))
				{
					if (IsActive)
						ScreenExtensions.TryActivate(ActiveItem);
				}
				else
				{
					if (ActiveItem != null)
						history.Add(ActiveItem);
					ChangeActiveItem(item, false);
				}
			}

			/// <summary>
			/// Deactivate the given item
			/// </summary>
			/// <param name="item">Item to deactivate</param>
			public override void DeactivateItem(T item)
			{
				ScreenExtensions.TryDeactivate(item);
			}

			/// <summary>
			/// Close the active item, and re-activate the top item in the history stack
			/// </summary>
			public void GoBack()
			{
				CloseItem(ActiveItem);
			}

			/// <summary>
			/// Close and remove all items in the history stack, leaving the ActiveItem
			/// </summary>
			public void Clear()
			{
				foreach (var item in history)
					this.CloseAndCleanUp(item, DisposeChildren);
				history.Clear();
			}

			/// <summary>
			/// Close the given item. If it was the ActiveItem, activate the top item in the history stack
			/// </summary>
			/// <param name="item">Item to close</param>
			public override async void CloseItem(T item)
			{
				if (item == null || !await CanCloseItem(item))
					return;

				if (item.Equals(ActiveItem))
				{
					var newItem = default(T);
					if (history.Count > 0)
					{
						newItem = history.Last();
						history.RemoveAt(history.Count - 1);
					}
					ChangeActiveItem(newItem, true);
				}
				else if (history.Contains(item))
				{
					this.CloseAndCleanUp(item, DisposeChildren);
					history.Remove(item);
				}
			}

			/// <summary>
			/// Returns true if and when all items (ActiveItem + everything in the history stack) can close
			/// </summary>
			/// <returns>A task indicating whether this conductor can close</returns>
			public override Task<bool> CanCloseAsync()
			{
				return CanAllItemsCloseAsync(history.Concat(new[] { ActiveItem }));
			}

			/// <summary>
			/// Ensures that all children are closed when this conductor is closed
			/// </summary>
			protected override void OnClose()
			{
				// We've already been deactivated by this point
				foreach (var item in history)
					this.CloseAndCleanUp(item, DisposeChildren);
				history.Clear();

				this.CloseAndCleanUp(ActiveItem, DisposeChildren);
			}
		}
	}
}
