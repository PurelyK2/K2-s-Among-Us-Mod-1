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
using System.Xml.Linq;
using TownOfUs;
using TownOfUs.Assets;
using TownOfUs.Buttons.Crewmate;
using TownOfUs.Extensions;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Game.Assailant;
using TownOfUs.Modules;
using TownOfUs.Modules.Wiki;
using TownOfUs.Patches;
using TownOfUs.Roles;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;

//Note: This Role Was Suggested By: ‧₊˚✧ 𝒥𝒶𝓎 :3 ✧˚₊‧ (Discord)
public sealed class DeceiverRole(IntPtr cppPtr) : ImpostorRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, ICrewVariant
{
    public static bool confuseRole;
    public static bool hasGameStarted;

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

    public override void OnRoleSet()
    {
        base.OnRoleSet();
		Player.AddModifier<DeceiverModifier>();
    }

    [HarmonyPatch(typeof(IntroScenePatches), "ShowTeamPatchPostfix", [typeof(IntroCutscene)])]
    public static class DeceiverWaitsPatch
    {
        public static void Postfix()
        {
            hasGameStarted = true;
            ReConfuse();
        }
    }

    [RegisterEvent(0)]
    public static void OnDeathEvent(AfterMurderEvent @event)
    {
        if(PlayerControl.LocalPlayer.Data.IsDead)
        {
            confuseRole = false;
            hasGameStarted = false;
        }
    }

    #region Deceiver Deceive Exceptions
    [HarmonyPatch(typeof(AssassinModifier), "ClickGuess", [typeof(PlayerVoteArea), typeof(MeetingHud)])]
	public static class AssassinGuessDeceiverPatch
	{
		public static void Prefix()
		{
			confuseRole = false;
		}
		public static void Postfix()
        {
            ReConfuse();
        }
    }

    [HarmonyPatch(typeof(TownOfUs.Utilities.Extensions), "RpcChangeRole", [typeof(PlayerControl), typeof(ushort), typeof(bool)])]
    public static class DeceiverChangesRolePatch
    {
        public static void Prefix()
        {
            confuseRole = false;
        }
        public static void Postfix()
        {
            ReConfuse();
        }
    }
    
    [HarmonyPatch(typeof(LogicGameFlowNormal), nameof(LogicGameFlowNormal.CheckEndCriteria))]
	public static class DeceiverDoesntDeceiveTheGamePatch
    {
        [HarmonyPriority(801)]
        public static void Prefix()
        {
            confuseRole = false;
        }
        public static void Postfix()
        {
            ReConfuse();
        }
    }

    [HarmonyPatch(typeof(TownOfUs.Utilities.Extensions), "IsImpostorAligned", [typeof(PlayerControl)])]
    public static class DeceiverIsImpostorPatch
    {
        public static void Prefix()
        {
            confuseRole = false;
        }
        public static void Postfix()
        {
            ReConfuse();
        }
    }
    #endregion

    [HarmonyPatch(typeof(NetworkedPlayerInfo), "Role", MethodType.Getter)]
    public static class GetDeceiverRolePatch
    {
		public static void Postfix(ref RoleBehaviour __result, ref NetworkedPlayerInfo __instance)
		{
			if (__result?.Player == null || __instance?.PlayerId == null) return;

			if(__result.Player.HasModifier<DeceiverModifier>())
			{
				__result = DestroyableSingleton<RoleManager>.Instance.GetRole((RoleTypes)RoleId.Get<DeceiverRole>());
			}

			if (!confuseRole) return;
			if (__result is not DeceiverRole || __instance.IsDead) return;
			if(__instance.AmOwner
				&& (__instance.IsDead
                || __result.Player.HasModifier<BaseRevealModifier>())) return;

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

    [RegisterEvent(0)]
	public static void EndGamePatch(BeforeGameEndEvent @event)
    {
        hasGameStarted = false;
        confuseRole = false;
	}

    [HarmonyPatch(typeof(AmongUsClient), "CoStartGame")]
    public static class DeceiverBreakingInsurance
    {
        public static void Prefix()
        {
            hasGameStarted = false;
            confuseRole = false;
        }
    }

    static void ReConfuse()
    {
        if (!hasGameStarted) return;
        if (PlayerControl.LocalPlayer.Data.Role.IsImpostor() || PlayerControl.LocalPlayer.Data.IsDead || PlayerControl.LocalPlayer.Data.Role is IGhostRole) return;
        if (PlayerControl.LocalPlayer.Data.Role is SnitchRole || PlayerControl.LocalPlayer.Data.Role is InquisitorRole) return;

        if(!confuseRole)
            Info("Confusing For Deceiver");

        confuseRole = true;
    }
}