using MiraAPI.Modifiers;
using TownOfUs.Assets;
using TownOfUs.Modifiers.Game;
using UnityEngine;
using K2AmongUs.Roles.Neutral;

namespace K2AmongUs.Modifiers.Neutral;

/// <inheritdoc/>
public sealed class ScrubberScrubModifier : BaseModifier
{
    /// <inheritdoc/>
    public override string ModifierName => "Scrubber Scrubbing";

    /// <inheritdoc/>
    public override void OnMeetingStart()
    {
        List<BaseModifier> modifiers = Player.GetModifiers<BaseModifier>().Where(m => !m.HideOnUi && !(m is AllianceGameModifier) && !(m is ScrubberScrubModifier)).ToList();
        List<string> modifierNames = new List<string>();

        if (modifiers.Count > 0) return;

        foreach(BaseModifier modifier in modifiers)
        {
            modifierNames.Add(modifier.ModifierName);
            Player.RemoveModifier(modifier);
        }

        if(Player.AmOwner)
        {
            MiraAPI.Utilities.Helpers.CreateAndShowNotification("Your modifiers have been scrubbed...", Color.yellow, new Vector3(0f, 1f, -20f), null, TouModifierIcons.Bait.LoadAsset());
        }

        if(PlayerControl.LocalPlayer.Data.Role is ScrubberRole scrubber)
        {
            MiraAPI.Utilities.Helpers.CreateAndShowNotification("You Have Cleansed " + Player.Data.PlayerName + " Of Their Modifiers!", Color.yellow, new Vector3(0f, 1f, -20f), null, TouModifierIcons.Bait.LoadAsset());
        }
    }
}