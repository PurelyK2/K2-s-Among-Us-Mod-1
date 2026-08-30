using System;
using System.Linq;
using System.Runtime.CompilerServices;
using MiraAPI.GameOptions;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using TouExtensionExample.Options.Roles.Crewmate;
using TouExtensionExample.Roles.Crewmate;
using TownOfUs.Assets;
using TownOfUs.Buttons;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.HnsCrewmate;
using TownOfUs.Modifiers.Impostor;
using TownOfUs.Modules.Localization;
using TownOfUs.Options.Roles.HnsCrewmate;
using TownOfUs.Options.Roles.Neutral;
using UnityEngine;
using K2AmongUs.Modifiers.Crewmate;

namespace K2AmongUs.Buttons.HideAndSeek.Hider;

public sealed class StealthySwoopButton : TownOfUsRoleButton<StealthyRole>
{
	public override Color TextOutlineColor => Color.black;
	public override string Name => "SNEAK";
	public override BaseKeybind Keybind => Keybinds.PrimaryAction;
	public override float Cooldown => Math.Clamp(OptionGroupSingleton<StealthyOptions>.Instance.SneakCooldown + TownOfUsButton.MapCooldown, 5f, 120f);

	public override float EffectDuration => OptionGroupSingleton<StealthyOptions>.Instance.SneakDuration;

	public override int MaxUses => (int)OptionGroupSingleton<StealthyOptions>.Instance.MaxSneaks;
	public override LoadableAsset<Sprite> Sprite => TouCrewAssets.CrewSwoopSprite;
	public override bool ZeroIsInfinite => true;

	public override bool CanUse()
	{
		if (DestroyableSingleton<HudManager>.Instance.Chat.IsOpenOrOpening || MeetingHud.Instance)
		{
			return false;
		}
		return !PlayerControl.LocalPlayer.GetModifiers<DisabledModifier>(null).Any((DisabledModifier x) => !x.CanUseAbilities) && ((Timer <= 0f && !EffectActive && (!LimitedUses || UsesLeft > 0)) || (EffectActive && Timer <= EffectDuration - 2f));
	}

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

    public override void OnEffectEnd()
    {
		Timer = Cooldown;
		EffectActive = false;
		PlayerControl.LocalPlayer.RpcRemoveModifier<StealthySwoopModifier>(null);
    }
}
