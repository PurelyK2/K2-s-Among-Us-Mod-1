using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Player;
using K2AmongUs.Roles.Crewmate;
using TownOfUs.Modules;
using MiraAPI.GameOptions;
using K2AmongUs.Options.Roles.Crewmate;

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
			if(@event.Player.GetRoleWhenAlive() is JackOfAllRole jackOfAllRole)
			{
				if(jackOfAllRole.NumTasksUntilMod <= 1)
				{
					JackOfAllRole.CheckAddModifier(@event.Player);
					jackOfAllRole.NumTasksUntilMod = (int)OptionGroupSingleton<JackOfAllOptions>.Instance.TasksPerMod;
				}
				else
				{
					jackOfAllRole.NumTasksUntilMod -= 1;
				}
			}
		}
	}
}