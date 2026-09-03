using K2AmongUs.Modifiers.Crewmate;
using K2AmongUs.Roles.Neutral;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using TownOfUs.Modules;
using TownOfUs.Utilities;

namespace K2AmongUs.Events.Neutral;

/// <inheritdoc/>
public static class MimicGameEndEvent
{
	/// <inheritdoc/>
	[RegisterEvent(0)]
	public static void OnRoundStart(RoundStartEvent @event)
	{
		foreach(PlayerControl player in MiraAPI.Utilities.Helpers.GetAlivePlayers().Where(p => p.GetRoleWhenAlive() is MimicRole).ToList())
		{
			if(player.AmOwner)
				(player.Data.Role as MimicRole)?.OpenPickingUI();
		}
	}

	/// <inheritdoc/>
	[RegisterEvent(0)]
	public static void GameEndEvent(GameEndEvent @event)
	{
		foreach(PlayerControl player in PlayerControl.AllPlayerControls.ToArray().Where(p => p.HasModifier<MimicRoleModifier>()).ToList())
		{
			player.RpcChangeRole(RoleId.Get<MimicRole>());
		}
	}
}