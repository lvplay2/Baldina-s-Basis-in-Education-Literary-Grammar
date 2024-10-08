using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Cleaner : NPC_NavMove
{
	public GameObject currentWater;

	private int punishTime;

	private IEnumerator punishCoroutine;

	private IEnumerator punishWaitCoroutine;

	protected override void Awake()
	{
		base.Awake();
		punishTime = GameplayManager.This.cleaner_punishTime;
		ActivateNav();
		SetNewState(NpcState.MarkerMove);
		StartAudioRandom();
	}

	public override void EnterMarker(Collider other)
	{
		if (_currentTargetPosition == other.transform.position)
		{
			if (ChangePlayerPunish(PunishChange.Check))
			{
				Sing_Game.This.playerController.EnableControlls(true);
			}
			if (punishWaitCoroutine != null)
			{
				StopCoroutine(punishWaitCoroutine);
			}
			currentWater.SetActive(false);
			currentWater = other.transform.GetChild(0).gameObject;
			currentWater.SetActive(true);
		}
		base.EnterMarker(other);
	}

	public override void PlayerAction()
	{
		if (ChangePlayerPunish(PunishChange.Check))
		{
			ChangePlayerPunish(PunishChange.Remove);
			Sing_Game.This.playerController.EnableControlls(true);
			currentWater.SetActive(false);
			Sing_Game.This.gameCanvas.PunishTimerEnable(false, punishTime);
			if (punishCoroutine != null)
			{
				StopCoroutine(punishCoroutine);
			}
		}
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
		if (punishNpcType.Contains(NpcType.Bully))
		{
			Sing_Game.This.npc_Bully.PlayerAction();
		}
		if (punishNpcType.Contains(NpcType.Security))
		{
			Sing_Game.This.npc_Security.PlayerAction();
		}
		if (punishNpcType.Contains(NpcType.Queen))
		{
			Sing_Game.This.npc_Queen.PlayerAction();
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
		Debug.Log("girl out");
		PlayerPunish2();
	}

	private IEnumerator PunishEnd()
	{
		yield return new WaitForSeconds(punishTime);
		PlayerAction();
	}

	protected override void UpdateState()
	{
		NpcState npcState = base.npcState;
		if (npcState == NpcState.MarkerMove)
		{
			SetRandomTarget();
		}
	}
}
