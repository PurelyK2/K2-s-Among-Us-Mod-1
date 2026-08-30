using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using K2AmongUs.Options.Roles.Neutral;
using K2AmongUs.Roles.Neutral;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.Types;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Rewired;
using TownOfUs.Assets;
using TownOfUs.Modifiers;
using TownOfUs.Modules;
using TownOfUs.Utilities;
using UnityEngine;

namespace K2AmongUs.Modifiers.Crewmate;

/// <inheritdoc/>
public sealed class ScrubberScrubModifier : BaseModifier
{
    /// <inheritdoc/>
    public override string ModifierName => "Scrubber Scrubbing";

    /// <inheritdoc/>
    public override void OnMeetingStart()
    {
        List<BaseModifier> modifiers = Player.GetModifiers<BaseModifier>().Where(m => !m.HideOnUi).ToList();

        foreach(BaseModifier modifier in modifiers)
        {
            Player.RemoveModifier(modifier);
        }

        if(Player.AmOwner)
        {
            MiraAPI.Utilities.Helpers.CreateAndShowNotification("Your modifiers have been scrubbed...", Color.yellow, new Vector3(0f, 1f, -20f), null, TouModifierIcons.Bait.LoadAsset());
        }
    }

}