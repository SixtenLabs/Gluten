﻿using System.Threading.Tasks;

namespace SixtenLabs.Gluten
{
	/// <summary>
	/// Conductor with a single active item, and no other items
	/// </summary>
	/// <typeparam name="T">Type of child to conduct</typeparam>
	public partial class Conductor<T> : ConductorBaseWithActiveItem<T> where T : class
	{
		/// <summary>
		/// Activate the given item, discarding the previous ActiveItem
		/// </summary>
		/// <param name="item">Item to active</param>
		public override async void ActivateItem(T item)
		{
			if (item != null && item.Equals(ActiveItem))
			{
				if (IsActive)
					ScreenExtensions.TryActivate(item);
			}
			else if (await CanCloseItem(ActiveItem))
			{
				// CanCloseItem is null-safe

				ChangeActiveItem(item, true);
			}
		}

		/// <summary>
		/// Deactive the given item
		/// </summary>
		/// <param name="item">Item to deactivate</param>
		public override void DeactivateItem(T item)
		{
			if (item != null && item.Equals(ActiveItem))
				ScreenExtensions.TryDeactivate(ActiveItem);
		}

		/// <summary>
		/// Close the given item
		/// </summary>
		/// <param name="item">Item to close</param>
		public override async void CloseItem(T item)
		{
			if (item == null || !item.Equals(ActiveItem))
				return;

			if (await CanCloseItem(item))
				ChangeActiveItem(default(T), true);
		}

		/// <summary>
		/// Determine if this conductor can close. Depends on whether the ActiveItem can close
		/// </summary>
		/// <returns>Task indicating whether this can be closed</returns>
		public override Task<bool> CanCloseAsync()
		{
			return CanCloseItem(ActiveItem);
		}
	}
}
