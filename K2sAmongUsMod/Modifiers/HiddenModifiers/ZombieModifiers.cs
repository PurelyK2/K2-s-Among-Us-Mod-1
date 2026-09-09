using AmongUs.GameOptions;
using K2AmongUs.Options.Roles.Neutral;
using K2AmongUs.Roles.Neutral;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using Reactor.Utilities;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Game;
using TownOfUs.Modules;
using TownOfUs.Networking;
using TownOfUs.Roles.Crewmate;
using UnityEngine;

namespace K2AmongUs.Modifiers.Neutral;

/// <inheritdoc/>
public sealed class ZombieRevealedModifier : BaseRevealModifier
{
    /// <inheritdoc/>
    public override string ModifierName => "Zombie Revealed";
    /// <inheritdoc/>
    public override ChangeRoleResult ChangeRoleResult { get; set; } = ChangeRoleResult.Nothing;
    /// <inheritdoc/>
    public override RoleBehaviour ShownRole => RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<ZombieRole>());

    /// <inheritdoc/>
    public override bool RevealRole => true;
    /// <inheritdoc/>
    public override bool Visible => true;
    /// <inheritdoc/>
    public override string ExtraRoleText => string.Empty;
}

/// <inheritdoc/>
public sealed class ZombieLeaderRevealedModifier : BaseRevealModifier
{
    /// <inheritdoc/>
    public override string ModifierName => "Zombie Leader Revealed";
    /// <inheritdoc/>
    public override ChangeRoleResult ChangeRoleResult { get; set; } = ChangeRoleResult.Nothing;
    /// <inheritdoc/>
    public override RoleBehaviour ShownRole => RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<ZombieLeaderRole>());

    /// <inheritdoc/>
    public override bool RevealRole => true;
    /// <inheritdoc/>
    public override bool Visible => PlayerControl.LocalPlayer.Data.Role is ZombieRole;
    /// <inheritdoc/>
    public override string ExtraRoleText => string.Empty;
}

public sealed class  ZombieAllianceModifier : AllianceGameModifier
{
    public override string ModifierName => "Zombie Alliance Modifier";
    public override bool HideOnUi => true;
    public override int GetAssignmentChance()
    {
        return 0;
    }

    /// <inheritdoc/>
    public override bool? DidWin(GameOverReason gameOverReason)
    {
        if (MiraAPI.Utilities.Helpers.GetAlivePlayers().Any(p => p.Data.Role is ZombieLeaderRole && !p.Data.IsDead))
        {
            int numNonZombies = MiraAPI.Utilities.Helpers.GetAlivePlayers().Count(p => !(p.Data.Role is ZombieLeaderRole || p.Data.Role is ZombieRole));
            int numZombies = PlayerControl.AllPlayerControls.ToArray().Count(p => p.Data.Role is ZombieRole || p.Data.Role is ZombieLeaderRole);

            return numZombies > numNonZombies && TownOfUs.Utilities.MiscUtils.KillersAliveCount == 0;
        }
        return false;
    }
}

public sealed class ZombieArrowModifier(DeadBody deadBody, Color color) : ArrowDeadBodyModifier(deadBody, color, 0)
{
    public override string ModifierName => "Zombie Arrow";

    // Zombie Leader Arrow
    [RegisterEvent(0)]
    public static void AfterMurderEventHandler(AfterMurderEvent @event)
    {
        if (!CustomRoleUtils.GetActiveRolesOfType<ZombieLeaderRole>().Any())
        {
            return;
        }

        if (!OptionGroupSingleton<ZombieOptions>.Instance.ZombieArrows)
        {
            return;
        }

        Coroutines.Start(CoCreateArrow(@event.Target));
        Coroutines.Start(TownOfUs.Utilities.MiscUtils.CoFlash(DestroyableSingleton<ZombieRole>.Instance.RoleColor));
    }

    private static System.Collections.IEnumerator CoCreateArrow(PlayerControl target)
    {
        var deadBody = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(x => x.ParentId == target.PlayerId);

        if (deadBody == null)
        {
            yield break;
        }

        foreach (var zombieRole in CustomRoleUtils.GetActiveRolesOfType<ZombieLeaderRole>().Select(x => x.Player))
        {
            if (zombieRole.AmOwner)
            {
                zombieRole.AddModifier<ZombieArrowModifier>(deadBody, Color.white);
            }
        }
    }
}