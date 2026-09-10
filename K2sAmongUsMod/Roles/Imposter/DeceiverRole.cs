using AmongUs.GameOptions;
using HarmonyLib;
using Hazel;
using Il2CppInterop.Runtime.Attributes;
using InnerNet;
using K2AmongUs.Assets;
using K2AmongUs.Options.Roles.Crewmate;
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
using TownOfUs.Utilities;
using UnityEngine;

//Note: This Role Was Suggested By: ‧₊˚✧ 𝒥𝒶𝓎 :3 ✧˚₊‧ (Discord)
public sealed class DeceiverRole(IntPtr cppPtr) : ImpostorRole(cppPtr), ITownOfUsRole, IWikiDiscoverable
{
    public static bool confuseRole = true;

    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorConcealing;
    public string RoleName => "Deceiver (Dev)";

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

    [HarmonyPatch(typeof(NetworkedPlayerInfo), "Role", MethodType.Getter)]
    public static class GetDeceiverRolePatch
    {
        public static void Postfix(ref RoleBehaviour __result, ref NetworkedPlayerInfo __instance)
        {
            if (!confuseRole) return;
            if (__result is not DeceiverRole || __instance.AmOwner || __result.IsImpostor()) return;
            
            __result = DestroyableSingleton<RoleManager>.Instance.GetRole((RoleTypes)RoleId.Get<InvestigatorRole>());
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