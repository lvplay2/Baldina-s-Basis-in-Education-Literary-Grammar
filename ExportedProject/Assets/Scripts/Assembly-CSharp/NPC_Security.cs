using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Security : NPC_TeleportMove
{
	private enum SecuritySoundList
	{
		Delicious = 0
	}

	private int stayTime;

	private int punishTime;

	private IEnumerator changePositionsCoroutine;

	private IEnumerator punishCoroutine;

	private IEnumerator punishWaitCoroutine;

	protected override void Awake()
	{
		base.Awake();
		stayTime = GameplayManager.This.security_stayTime;
		punishTime = GameplayManager.This.security_punishTime;
		SetNewState(NpcState.MarkerMove);
		StartAudioRandom();
	}

	public override void PlayerAction()
	{
		if (ChangePlayerPunish(PunishChange.Check))
		{
			PlaySound(SecuritySoundList.Delicious);
			ChangePlayerPunish(PunishChange.Remove);
			if (punishCoroutine != null)
			{
				StopCoroutine(punishCoroutine);
			}
			Sing_Game.This.playerController.EnableControlls(true);
			Sing_Game.This.gameCanvas.PunishTimerEnable(false);
			if (punishCoroutine != null)
			{
				StopCoroutine(punishCoroutine);
			}
			SetNewState(NpcState.MarkerMove);
		}
	}

	public override void PlayerCollision()
	{
		SetNewState(NpcState.PlayerPunish);
	}

	public override void PlayerPunish()
	{
		List<NpcType> punishNpcType = Sing_Game.This.playerController.punishNpcType;
		if (punishNpcType.Contains(NpcType.Girl))
		{
			if (punishWaitCoroutine != null)
			{
				StopCoroutine(punishWaitCoroutine);
			}
			punishWaitCoroutine = PunishWait();
			StartCoroutine(punishWaitCoroutine);
		}
		else
		{
			PlayerPunish2();
		}
	}

	private void PlayerPunish2()
	{
		List<NpcType> punishNpcType = Sing_Game.This.playerController.punishNpcType;
		if (punishNpcType.Contains(NpcType.Cleaner))
		{
			Sing_Game.This.npc_Cleaner.PlayerAction();
		}
		if (punishNpcType.Contains(NpcType.Thed))
		{
			Sing_Game.This.npc_Thed.PlayerAction();
		}
		ChangePlayerPunish(PunishChange.Add);
		Sing_Game.This.playerController.EnableControlls(false);
		Sing_Game.This.gameCanvas.PunishTimerEnable(true, punishTime);
		if (punishCoroutine != null)
		{
			StopCoroutine(punishCoroutine);
		}
		punishCoroutine = PunishEnd();
		StartCoroutine(punishCoroutine);
	}

	private IEnumerator PunishWait()
	{
		List<NpcType> punishTypes = Sing_Game.This.playerController.punishNpcType;
		while (punishTypes.Contains(NpcType.Girl))
		{
			yield return new WaitForSeconds(0.1f);
			punishTypes = Sing_Game.This.playerController.punishNpcType;
		}
		PlayerPunish2();
	}

	private IEnumerator PunishEnd()
	{
		yield return new WaitForSeconds(punishTime);
		PlayerAction();
	}

	protected override void UpdateState()
	{
		switch (npcState)
		{
		case NpcState.MarkerMove:
			EnablePositions(true);
			break;
		case NpcState.PlayerPunish:
			EnablePositions(false);
			PlayerPunish();
			break;
		}
	}

	public void EnablePositions(bool isOn)
	{
		if (changePositionsCoroutine != null)
		{
			StopCoroutine(changePositionsCoroutine);
		}
		if (isOn)
		{
			changePositionsCoroutine = ChangePosition();
			StartCoroutine(changePositionsCoroutine);
		}
	}

	private IEnumerator ChangePosition()
	{
		while (true)
		{
			SetRandomPosition();
			yield return new WaitForSeconds(stayTime);
		}
	}

	private void PlaySound(SecuritySoundList _sound)
	{
		StartAudioRandom(false);
		audioSourceMain.clip = audioStoryClips[(int)_sound];
		audioSourceMain.Play();
		StartAudioRandom();
	}
}
