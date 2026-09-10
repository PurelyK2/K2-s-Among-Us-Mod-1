using AmongUs.GameOptions;
using HarmonyLib;
using Hazel;
using Il2CppInterop.Runtime.Attributes;
using InnerNet;
using K2AmongUs.Assets;
using K2AmongUs.Options.Roles.Crewmate;
using K2AmongUs.Options.Roles.Impostor;
using K2sAmongUsMod.Modifiers.HiddenModifiers;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using TownOfUs;
using TownOfUs.Assets;
using TownOfUs.Buttons.Crewmate;
using TownOfUs.Extensions;
using TownOfUs.Modifiers.Game.Assailant;
using TownOfUs.Modules;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

//Note: This Role Was Suggested By: ‧₊˚✧ 𝒥𝒶𝓎 :3 ✧˚₊‧ (Discord)
public sealed class DeceiverRole(IntPtr cppPtr) : ImpostorRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, ICrewVariant
{
    public static bool confuseRole;

    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorConcealing;
    public string RoleName => "Deceiver (Dev)";
    public string LocaleKey => "Deceiver";

    public string RoleDescription => "You Seem Innocent To Others...";
    public string RoleLongDescription => "Appear As A Random, Not-In-Play Crewmate Role To Those Collecting Information From You.";
	public string GetAdvancedDescription()
	{
		return RoleLongDescription + MiscUtils.AppendOptionsText(base.GetType());
	}

	public Color RoleColor => TownOfUsColors.Impostor;

    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;

    public CustomRoleConfiguration Configuration => new(this)
    {
        UseVanillaKillButton = true,
        IntroSound = TouAudio.HackedSound,
        Icon = K2RoleIcons.Deceiver
    };

    public RoleBehaviour CrewVariant => DestroyableSingleton<RoleManager>.Instance.GetRole((RoleTypes)RoleId.Get<SeerRole>());

    public override void OnRoleSet()
    {
        base.OnRoleSet();
		Player.AddModifier<DeceiverModifier>();
    }

    [RegisterEvent(0)]
    public static void OnRoundStartEventHandler(RoundStartEvent _)
    {
		confuseRole = true;
    }

	[HarmonyPatch(typeof(AssassinModifier), "ClickGuess", [typeof(PlayerVoteArea), typeof(MeetingHud)])]
	public static class AssassinGuessDeceiverPatch
	{
		public static void Prefix()
		{
			confuseRole = false;
		}
		public static void Postfix()
		{
			confuseRole = true;
		}
	}

    [HarmonyPatch(typeof(NetworkedPlayerInfo), "Role", MethodType.Getter)]
    public static class GetDeceiverRolePatch
    {
		public static void Postfix(ref RoleBehaviour __result, ref NetworkedPlayerInfo __instance)
		{
			/*
			Skip if one of the following:
			1. Game Is At A Point Where It Shouldn't Confuse
				* The Game Hasn't Started
				* The Game Is Ending
				* The Player Is Guessing Your Role (Assassin, Vigilante, Doomsayer, etc.)
				- Someone is checking for your role (sleuth, imitator, mimic, etc.)
			2. __result isn't Deceiver
			3. You Are The Owner of a player AND One Of The Following:
				- You Are Imp
				- You Are Dead
				- You Are Snitch
				- You Are Inquisitor
			 */
			if(MiscUtils.PlayerById(__instance.PlayerId).HasModifier<DeceiverModifier>())
			{
				__result = DestroyableSingleton<RoleManager>.Instance.GetRole((RoleTypes)RoleId.Get<DeceiverRole>());
				
				if (__instance.AmOwner) return;
			}

			if (!confuseRole) return;
			if (__result is not DeceiverRole || __instance.IsDead) return;
			if(__instance.AmOwner
				&& (__instance.Role.IsImpostor()
				|| __instance.IsDead
				|| __instance.Role is SnitchRole
				|| __instance.Role is InquisitorRole)) return;

			if(OptionGroupSingleton<DeceiverOptions>.Instance.DeceiverDisplayedAs == DeceiverOptions.DeceiverRoleDisplayed.Investigator)
				__result = DestroyableSingleton<RoleManager>.Instance.GetRole((RoleTypes)RoleId.Get<InvestigatorRole>());
			else
			{
				List<RoleBehaviour> crewRoles = DestroyableSingleton<RoleManager>.Instance.AllRoles.ToArray().Where(r => r.IsCrewmate() && r.GetRoleAlignment() != RoleAlignment.CrewmateKilling).ToList();

				crewRoles.RemoveAll(r => PlayerControl.AllPlayerControls.ToArray().Any(p => p.Data.Role.GetType() == r.GetType()));

				if(crewRoles.Count > 0)
				{
                    __result = DestroyableSingleton<RoleManager>.Instance.GetRole((RoleTypes)RoleId.Get<InvestigatorRole>());
					return;
                }

				RoleBehaviour randomCrewRole = crewRoles[UnityEngine.Random.Range(0, crewRoles.Count)];

				if(randomCrewRole != null )
				{
                    __result = randomCrewRole;
                }
			}

		}
	}

	[HarmonyPatch(typeof(InnerNetClient), "StartEndGame")]
	public static class EndGamePatch
	{
		public static void Prefix()
		{
			confuseRole = false;
		}
	}
}