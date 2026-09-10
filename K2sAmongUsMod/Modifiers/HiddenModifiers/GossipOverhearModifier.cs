using MiraAPI.Modifiers;
using K2AmongUs.Roles.Crewmate;
using TownOfUs.Modules;
using MiraAPI.GameOptions;
using K2AmongUs.Options.Roles.Crewmate;
using MiraAPI.Roles;
using TownOfUs.Utilities;
using MiraAPI.Utilities;
using AmongUs.GameOptions;
using TMPro;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles;

namespace K2AmongUs.Modifiers.Crewmate;

/// <inheritdoc/>
public sealed class GossipOverhearModifier : BaseModifier
{
    /// <inheritdoc/>
    public GossipOverhearModifier(List<RoleBehaviour> rolesList)
    {
        GossipRoles = rolesList;
    }
    public GossipOverhearModifier(RoleBehaviour[] rolesList)
    {
        GossipRoles = rolesList.ToList();
    }
    public GossipOverhearModifier(string rolesList)
    {
        string[] roleNames = rolesList.Split("|");

        GossipRoles = roleNames.Select(name => DestroyableSingleton<RoleManager>.Instance.AllRoles.ToArray().First(r => r.GetRoleName() == name)).ToList();
    }

    /// <inheritdoc/>
    public List<RoleBehaviour> GossipRoles = [];

    /// <inheritdoc/>
    public override string ModifierName => "Gossip Target";
    /// <inheritdoc/>
    public override bool HideOnUi => true;

    /// <inheritdoc/>
    public override void OnActivate()
    {
        base.OnActivate();

        foreach(GossipOverhearModifier? gossipOverhearModifier in PlayerControl.AllPlayerControls.ToArray().Where(p => p.HasModifier<GossipOverhearModifier>()).Select(p => p.GetModifier<GossipOverhearModifier>()))
        {
            if(gossipOverhearModifier != this)
            {
                gossipOverhearModifier?.Player.RemoveModifier<GossipOverhearModifier>();
            }
        }
    }

    /// <inheritdoc/>
    public override void OnDeath(DeathReason reason)
    {
        base.OnDeath(reason);

        Player.RemoveModifier(this);
    }

    /// <inheritdoc/>
    public override void OnMeetingStart()
    {
        if(!MiraAPI.Utilities.Helpers.GetAlivePlayers().Any(p => p.GetRoleWhenAlive() is GossipRole)) return;

        if(Player == null)
        {
            Error("Player Is Null For Gossip");
            return;
        }        
        
        GossipRole.GenerateGossip(Player, GossipRoles);
        Player.RemoveModifier<GossipOverhearModifier>();

        GossipRoles = new List<RoleBehaviour>();
    }

    /// <inheritdoc/>
    public static RoleBehaviour[] GenerateGossipRoles(PlayerControl player)
    {
        int randRolesCount = (int)OptionGroupSingleton<GossipOptions>.Instance.GossipRoles;

        List<RoleBehaviour> allRoles = DestroyableSingleton<RoleManager>.Instance.AllRoles.ToArray().Where(r => r is not IGhostRole).ToList();
        List<RoleBehaviour> possibleRoles = new List<RoleBehaviour>();
        foreach(RoleBehaviour role in allRoles)
        {
            RoleManager.RoleAssignmentData roleData = CustomRoleUtils.GetAssignData(role.Role);
            if(roleData.Chance > 0 && roleData.Count > 0 && CustomRoleUtils.CanSpawnOnCurrentMode(role))
            {
                possibleRoles.Add(role);
            }
        }


        List<RoleBehaviour> randomRolesList = new List<RoleBehaviour>();

        if (player.HasModifier<ImitatorCacheModifier>())
        {
            RoleBehaviour imitatorRole = possibleRoles.First(r => r is ImitatorRole);
            randomRolesList.Add(imitatorRole);
            possibleRoles.Remove(imitatorRole);
        }
        else
        {
            possibleRoles.RemoveAll(role => role.GetType() == player.GetRoleWhenAlive().GetType());
            randomRolesList.Add(player.Data.Role);
        }

        for(int i = 0; i < randRolesCount; i++)
        {
            if(possibleRoles.Count == 0)
            {
                Error("No Roles For Gossip To Add");
                break;
            }

            List<RoleBehaviour> thesePossibleRoles = possibleRoles;

            //Weighted To Crewmate
            if(UnityEngine.Random.Range(0, 101) <= OptionGroupSingleton<GossipOptions>.Instance.CrewWeight)
            {
                thesePossibleRoles.RemoveAll(r => !r.IsCrewmate());
            }

            RoleBehaviour newRole = thesePossibleRoles[UnityEngine.Random.Range(0, possibleRoles.Count)];

            possibleRoles.Remove(newRole);
            randomRolesList.Add(newRole);
        }
        
        return randomRolesList.ToArray();
    }
}