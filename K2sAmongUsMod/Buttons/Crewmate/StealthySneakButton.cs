using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using K2AmongUs.Options.Roles.Crewmate;
using K2AmongUs.Roles.Crewmate;
using TownOfUs.Assets;
using TownOfUs.Buttons;
using TownOfUs.Modifiers;
using UnityEngine;
using K2AmongUs.Modifiers.Crewmate;

namespace K2AmongUs.Buttons.HideAndSeek.Hider;

/// <inheritdoc/>
public sealed class StealthySwoopButton : TownOfUsRoleButton<StealthyRole>
{
    /// <inheritdoc/>
	public override Color TextOutlineColor => Color.black;
    /// <inheritdoc/>
	public override string Name => "SNEAK";
    /// <inheritdoc/>
	public override BaseKeybind Keybind => Keybinds.PrimaryAction;
    /// <inheritdoc/>
	public override float Cooldown => Math.Clamp(OptionGroupSingleton<StealthyOptions>.Instance.SneakCooldown + TownOfUsButton.MapCooldown, 5f, 120f);

    /// <inheritdoc/>
	public override float EffectDuration => OptionGroupSingleton<StealthyOptions>.Instance.SneakDuration;

    /// <inheritdoc/>
	public override int MaxUses => (int)OptionGroupSingleton<StealthyOptions>.Instance.MaxSneaks;
    /// <inheritdoc/>
	public override LoadableAsset<Sprite> Sprite => TouCrewAssets.CrewSwoopSprite;
    /// <inheritdoc/>
	public override bool ZeroIsInfinite => true;

    /// <inheritdoc/>
	public override bool CanUse()
	{
		if (DestroyableSingleton<HudManager>.Instance.Chat.IsOpenOrOpening || MeetingHud.Instance)
		{
			return false;
		}
		return !PlayerControl.LocalPlayer.GetModifiers<DisabledModifier>(null).Any((DisabledModifier x) => !x.CanUseAbilities) && ((Timer <= 0f && !EffectActive && (!LimitedUses || UsesLeft > 0)) || (EffectActive && Timer <= EffectDuration - 2f));
	}

    /// <inheritdoc/>
	protected override void OnClick()
	{
		EffectActive = true;
		PlayerControl.LocalPlayer.RpcAddModifier<StealthySwoopModifier>();

		int usesLeft = UsesLeft;
		UsesLeft = usesLeft - 1;
		if (LimitedUses && !EffectActive)
		{
			if (Button == null)
			{
				return;
			}
			Button.SetUsesRemaining(UsesLeft);
		}
	}

    /// <inheritdoc/>
    public override void OnEffectEnd()
    {
		Timer = Cooldown;
		EffectActive = false;
		PlayerControl.LocalPlayer.RpcRemoveModifier<StealthySwoopModifier>(null);
    }
}
