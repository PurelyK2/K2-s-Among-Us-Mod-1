using MiraAPI.Modifiers;
using K2AmongUs.Roles.Crewmate;
using TownOfUs.Modules;

namespace K2AmongUs.Modifiers.Crewmate;

/// <inheritdoc/>
public sealed class GossipOverhearModifier : BaseModifier
{
    /// <inheritdoc/>
    public override string ModifierName => "Gossip Target";
    /// <inheritdoc/>
    public override bool HideOnUi => true;

    /// <inheritdoc/>
    public override void OnMeetingStart()
    {
        if(!MiraAPI.Utilities.Helpers.GetAlivePlayers().Any(p => p.GetRoleWhenAlive() is GossipRole)) return;

        if(Player == null)
        {
            Error("Player Is Null For Gossip");
            return;
        }        
        
        GossipRole.GenerateGossip(Player);
        Player.RemoveModifier<GossipOverhearModifier>();
    }

}