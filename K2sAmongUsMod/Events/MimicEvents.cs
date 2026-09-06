using HarmonyLib;
using K2AmongUs.Modifiers.Neutral;
using K2AmongUs.Roles.Neutral;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using TownOfUs.Events.TouEvents;
using TownOfUs.Utilities;

namespace TownOfUs.Events.Crewmate;

public static class MimicEvents
{
    [RegisterEvent(-1000)]
    public static void RoundStartEventHandler(RoundStartEvent @event)
    {
        if (@event.TriggeredByIntro)
        {
            return;
        }

        var MimicRoles = CustomRoleUtils.GetActiveRolesOfType<MimicRole>();

        if (MimicRoles.HasAny())
        {
            foreach (var MimicPlayer in MimicRoles)
            {
                if (MimicPlayer.Player.HasModifier<MimicCacheModifier>())
                {
                    continue;
                }

                MimicPlayer.Player.AddModifier<MimicCacheModifier>();
            }
        }

        var Mimics = ModifierUtils.GetActiveModifiers<MimicCacheModifier>(x => !x.Player.HasDied() && x.Player.IsCrewmate());

        if (!Mimics.HasAny())
        {
            return;
        }

        foreach (var mod in Mimics)
        {
            if (mod.Player.AmOwner)
            {
                mod.UpdateRole();
            }
        }
    }

    [HarmonyPatch(typeof(MeetingHud), "Start")]
    [HarmonyPrefix]
    public static void FixMimicRole()
    {
        foreach(PlayerControl player in PlayerControl.AllPlayerControls)
        {
            if (player.HasModifier<MimicCacheModifier>())
            {
                player.RpcChangeRole(RoleId.Get<MimicRole>(), false);
            }
        }
    }

    [RegisterEvent]
    public static void ChangeRoleHandler(ChangeRoleEvent @event)
    {
        if (!PlayerControl.LocalPlayer)
        {
            return;
        }

        var player = @event.Player;

        if (player.HasModifier<MimicCacheModifier>() && !(@event.NewRole is MimicRole))
        {
            var text = "Removed Mimic Cache Modifier On Role Change";
            MiscUtils.LogInfo(TownOfUsEventHandlers.LogLevel.Error, text);

            player.RemoveModifier<MimicCacheModifier>();
        }
    }
}