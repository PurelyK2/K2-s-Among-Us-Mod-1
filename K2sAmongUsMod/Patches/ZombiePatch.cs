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
    public static void OnMeetingStart(StartMeetingEvent @event)
    {
        foreach (PlayerControl player in MiraAPI.Utilities.Helpers.GetAlivePlayers().Where(p => p.Data.Role is ZombieRole))
        {
            player.Data.IsDead = true;
        }
    }

    /// <inheritdoc/>
    [RegisterEvent(0)]
    public static void OnRoundStart(RoundStartEvent @event)
    {
        if (!MiraAPI.Utilities.Helpers.GetAlivePlayers().Any(p => p.Data.Role is ZombieLeaderRole))
        {
            Info("No Zombie Leader Found, Remain Dead");
        }

        foreach (PlayerControl player in PlayerControl.AllPlayerControls.ToArray().Where(p => p.Data.Role is ZombieRole))
        {
            Info("Reviving Zombie: " + player.Data.PlayerName);
            player.Data.IsDead = false;
        }
    }
}