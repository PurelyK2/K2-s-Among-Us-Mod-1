using TownOfUs.Modules;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Meeting;
using K2AmongUs.Roles.Neutral;
using TownOfUs.Networking;
using Il2CppSystem.Web.Util;

namespace K2AmongUs.Patches;

/// <inheritdoc/>
public static class ZombiePatches
{
    /// <inheritdoc/>
    [RegisterEvent(0)]
    public static void HandleVoteEvent(MiraAPI.Events.Vanilla.Meeting.Voting.HandleVoteEvent @event)
    {
        ZombieLeaderRole? leader = @event.VoteData.Owner.Data.Role as ZombieLeaderRole;
        if (leader != null)
        {
            @event.VoteData.SetRemainingVotes(0);
            for (int i = 0; i < PlayerControl.AllPlayerControls.ToArray().Count(p => p.GetRoleWhenAlive() is ZombieRole) + 1; i++)
            {
                @event.VoteData.VoteForPlayer(@event.TargetId);
            }
            @event.Cancel();
        }
    }

    /// <inheritdoc/>
    [RegisterEvent(0)]
	public static void OnRoundStart(RoundStartEvent @event)
    {
        if(!PlayerControl.AllPlayerControls.ToArray().Any(p => p.GetRoleWhenAlive() is ZombieLeaderRole)) return;

        foreach (PlayerControl player in PlayerControl.AllPlayerControls.ToArray().Where(p => p.Data.Role is ZombieRole))
        {
            player.Data.IsDead = false;
        }
    }

    /// <inheritdoc/>
    [RegisterEvent(0)]
    public static void OnMeetingStart(StartMeetingEvent @event)
    {
        foreach(PlayerControl player in MiraAPI.Utilities.Helpers.GetAlivePlayers().Where(p => p.Data.Role is ZombieRole))
        {
            player.Data.IsDead = true;
        }
    }
}