using MiraAPI.Modifiers;
using TownOfUs.Assets;
using TownOfUs.Modifiers.Game;
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
        List<BaseModifier> modifiers = Player.GetModifiers<BaseModifier>().Where(m => !m.HideOnUi && !(m is AllianceGameModifier)).ToList();

        foreach(BaseModifier modifier in modifiers)
        {
            Player.RemoveModifier(modifier);
        }

        Player.RemoveModifier(this);

        if(Player.AmOwner)
        {
            MiraAPI.Utilities.Helpers.CreateAndShowNotification("Your modifiers have been scrubbed...", Color.yellow, new Vector3(0f, 1f, -20f), null, TouModifierIcons.Bait.LoadAsset());
        }
    }

}