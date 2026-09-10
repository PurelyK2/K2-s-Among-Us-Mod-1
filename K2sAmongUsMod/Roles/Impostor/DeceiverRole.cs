using AmongUs.GameOptions;
using HarmonyLib;
using Hazel;
using Il2CppInterop.Runtime.Attributes;
using InnerNet;
using K2AmongUs.Assets;
using K2AmongUs.Options.Roles.Crewmate;
using K2AmongUs.Options.Roles.Impostor;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using TownOfUs;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modules;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

//Note: This Role Was Suggested By: ‧₊˚✧ 𝒥𝒶𝓎 :3 ✧˚₊‧ (Discord)
public sealed class DeceiverRole(IntPtr cppPtr) : ImpostorRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, ICrewVariant, IUnguessable
{
    public static bool confuseRole = true;

    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorConcealing;
    public string RoleName => "Deceiver";
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

    public RoleBehaviour AppearAs => DestroyableSingleton<RoleManager>.Instance.GetRole((RoleTypes)RoleId.Get<SeerRole>());

    public bool IsGuessable => PlayerControl.LocalPlayer.Data.Role is not VigilanteRole && PlayerControl.LocalPlayer.Data.Role is not DoomsayerRole;

    [HarmonyPatch(typeof(NetworkedPlayerInfo), "Role", MethodType.Getter)]
    public static class GetDeceiverRolePatch
    {
		public static void Postfix(ref RoleBehaviour __result, ref NetworkedPlayerInfo __instance)
		{
			if (!confuseRole) return;
			if (__result is not DeceiverRole
				|| __instance.AmOwner &&
				(__instance.Role.IsImpostor()
				|| __instance.IsDead
				|| __instance.Role is SnitchRole
				|| __instance.Role is InquisitorRole
				|| __instance.Role is DoomsayerRole
				|| __instance.Role is VigilanteRole)) return;

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

	[HarmonyPatch(typeof(InnerNetServer), "StartGame", new[] { typeof(MessageReader), typeof(InnerNetServer.Player) })]
	public static class StartGamePatch
	{
		public static void Postfix()
		{
			confuseRole = true;
		}
	}

	[HarmonyPatch(typeof(InnerNetServer), "EndGame", new[] { typeof(MessageReader), typeof(InnerNetServer.Player) })]
	public static class EndGamePatch
	{
		public static void Prefix()
		{
			confuseRole = false;
		}
	}
}