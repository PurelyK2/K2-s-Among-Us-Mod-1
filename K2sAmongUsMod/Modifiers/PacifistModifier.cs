using AmongUs.GameOptions;
using HarmonyLib;
using K2AmongUs.Options.Modifiers.UniversalModifierOptions;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownOfUs.Modifiers.Game;
using TownOfUs.Roles;
using TownOfUs.Utilities;

namespace K2AmongUs.Modifiers;

public sealed class PacifistModifier : AllianceGameModifier
{
    public override string ModifierName => "Pacifist";
    public override string IntroInfo => "You Win With Crew";
    public override string Symbol => "+";
    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role.IsNeutral();
    }
    public override AlliedFaction TrueFactionType => AlliedFaction.Crewmate;
    public override bool CrewContinuesGame => false;
    public override string GetDescription()
    {
        return IntroInfo;
    }

    public override int GetAssignmentChance()
    {
        return (int)OptionGroupSingleton<PacifistOptions>.Instance.PacifistChance;
    }

    public override bool? DidWin(GameOverReason reason)
    {
        return reason == GameOverReason.CrewmatesByVote || reason == GameOverReason.CrewmatesByTask;
    }

    [HarmonyPatch(typeof(TownOfUs.Utilities.Extensions), "Is", [typeof(PlayerControl), typeof(RoleAlignment)])]
    public static class PacifistIsntEvilPatch
    {
        public static void Postfix(ref PlayerControl player, ref RoleAlignment roleAlignment, ref bool __result)
        {
            if(player.HasModifier<PacifistModifier>())
            {
                if (roleAlignment == RoleAlignment.CrewmateKilling) __result = true;
                else if (roleAlignment == RoleAlignment.NeutralKilling) __result = false;
            }
        }
    }
}
