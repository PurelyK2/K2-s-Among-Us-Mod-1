
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using K2AmongUs.Roles.Neutral;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.GameEnd;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using TMPro;
using TownOfUs;
using TownOfUs.Interfaces;
using TownOfUs.Modules;
using UnityEngine;

namespace K2AmongUs.Patches.WinConditions;

/// <inheritdoc/>
public sealed class ZombieGameOver : CustomGameOver
{
	/// <inheritdoc/>
	public override bool VerifyCondition(PlayerControl playerControl, NetworkedPlayerInfo[] winners)
	{
		return !Helpers.GetAlivePlayers().Any(p => !(p.GetRoleWhenAlive() is ZombieRole || p.GetRoleWhenAlive() is ZombieLeaderRole));
	}

	/// <inheritdoc/>
	public override void AfterEndGameSetup(EndGameManager endGameManager)
	{
		endGameManager.BackgroundBar.material.SetColor(ShaderID.Color, new Color32(84, 192, 113, byte.MaxValue));
		TextMeshPro text = UnityEngine.Object.Instantiate<TextMeshPro>(endGameManager.WinText);
		text.text = "Zombies Win!";
		text.color = new Color32(84, 192, 113, byte.MaxValue);
		DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 2);
		defaultInterpolatedStringHandler.AppendLiteral("<color=#");
		defaultInterpolatedStringHandler.AppendFormatted(new Color32(84, 192, 113, byte.MaxValue));
		defaultInterpolatedStringHandler.AppendLiteral(">");
        defaultInterpolatedStringHandler.AppendLiteral("Zombies Win!</color>");
        GameHistory.WinningFaction = defaultInterpolatedStringHandler.ToStringAndClear();
		Vector3 pos = endGameManager.WinText.transform.localPosition;
		pos.y = 1.5f;
		pos += Vector3.down * 0.15f;
		text.transform.localScale = new Vector3(1f, 1f, 1f);
		text.transform.position = pos;
		text.text = "<size=4>" + text.text + "</size>";
	}
}