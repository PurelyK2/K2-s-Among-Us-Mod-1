using TownOfUs.Modules;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Meeting;
using K2AmongUs.Roles.Neutral;
using TownOfUs.Networking;
using Il2CppSystem.Web.Util;
using HarmonyLib;

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
        if (!MiraAPI.Utilities.Helpers.GetAlivePlayers().Any(p => p.Data.Role is ZombieLeaderRole))
        {
            Info("No Zombie Leader Found, Zombies Remain Dead");
        }

        foreach (PlayerControl player in PlayerControl.AllPlayerControls.ToArray().Where(p => p.Data.Role is ZombieRole && p.AmOwner))
        {
            Info("Reviving Zombie: " + player.Data.PlayerName);
            player.RpcBasicRevive();
        }
    }

    /// <inheritdoc/>
    [RegisterEvent(0)]
    public static void OnMeetingStart(StartMeetingEvent @event)
    {
        foreach (PlayerControl player in PlayerControl.AllPlayerControls.ToArray().Where(p => p.Data.Role is ZombieRole && p.AmOwner))
        {
            Info("Killing Zombie: " + player.Data.PlayerName);

            player.RpcSelfMurder(player, player, true, true, false, false, false, false, "Undead");
        }
    }

    [HarmonyPatch(typeof(ChatController), "AddChat", [typeof(PlayerControl), typeof(string), typeof(bool)])]
    public static class ZombiesChatPatch
    {
        public static void Prefix(ref PlayerControl sourcePlayer)
        {
            if (sourcePlayer.Data.Role is ZombieRole)
            {
                sourcePlayer.Data.IsDead = true;
            }
            if (PlayerControl.LocalPlayer.Data.Role is ZombieRole)
            {
                PlayerControl.LocalPlayer.Data.IsDead = true;
            }
        }

        //Only Do Postfix If You Somehow Send A Chat Outside Of A Meeting
        public static void Postfix(ref PlayerControl sourcePlayer)
        {
            if (sourcePlayer.Data.Role is ZombieRole && MeetingHud.Instance == null)
            {
                sourcePlayer.Data.IsDead = false;
            }
            if (PlayerControl.LocalPlayer.Data.Role is ZombieRole && MeetingHud.Instance == null)
            {
                PlayerControl.LocalPlayer.Data.IsDead = false;
            }
        }
    }
}