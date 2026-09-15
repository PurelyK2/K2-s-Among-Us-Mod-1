using AmongUs.GameOptions;
using HarmonyLib;
using K2AmongUs.Assets;
using K2AmongUs.Options.Modifiers.UniversalModifierOptions;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TownOfUs.Assets;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Game;
using TownOfUs.Roles;
using TownOfUs.Utilities;
using UnityEngine;

namespace K2AmongUs.Modifiers;

//Idea By Max
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
    public override ModifierUiConfiguration Configuration
    {
        get
        {
            return new ModifierUiConfiguration(new UnityEngine.Color32(120, 81, 169, Byte.MaxValue), TmpSpriteUtils.CreateSpriteAsset(TouRoleIcons.Agent.LoadAsset(), "TouMira.Modifier.Neutral.Pacifist", 1.45f));
        }
    }
    public override LoadableAsset<Sprite> ModifierIcon => TouRoleIcons.Agent;

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
