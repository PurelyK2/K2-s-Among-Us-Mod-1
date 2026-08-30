using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Player;
using K2AmongUs.Roles.Crewmate;

namespace K2AmongUs.Events.Crewmate
{
	/// <inheritdoc/>
	public static class JackOfAllEvents
	{
		/// <inheritdoc/>
		[RegisterEvent(0)]
		public static void CompleteTaskEvent(CompleteTaskEvent @event)
		{
			Info("Completed Task");
			JackOfAllRole.CheckAddModifier(@event.Player);
		}
	}
}