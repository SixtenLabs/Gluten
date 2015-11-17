namespace SixtenLabs.Gluten
{
	/// <summary>
	/// Generalised 'screen' composing all the behaviours expected of a screen
	/// </summary>
	public interface IScreen : IViewAware, IHaveDisplayName, IScreenState, IChild, IGuardClose, IRequestClose
	{
	}
}
