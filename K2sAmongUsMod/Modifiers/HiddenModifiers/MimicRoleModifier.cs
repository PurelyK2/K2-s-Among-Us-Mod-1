using K2AmongUs.Roles.Neutral;
using MiraAPI.Roles;
using TownOfUs.Modifiers.Game;
using TownOfUs.Utilities;

namespace K2AmongUs.Modifiers.Crewmate;
    
/// <inheritdoc/>
public sealed class MimicRoleModifier : AllianceGameModifier
{
    /// <inheritdoc/>
    public override string ModifierName => "Mimic";
    /// <inheritdoc/>
    public override bool HideOnUi => false;

    /// <inheritdoc/>
    public override int GetAssignmentChance()
    {
        return 0;
    }
    
    /// <inheritdoc/>
    public override void OnMeetingStart()
    {
        ReMimic();
    }

    /// <inheritdoc/>
    public override void OnDeath(DeathReason reason)
    {
        ReMimic();
    }

    void ReMimic()
    {
        Player.RpcChangeRole(RoleId.Get<MimicRole>(), false);
    }

}