using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Bully : NPC_NavMove
{
	private enum BullySoundList
	{
		Looser = 0,
		SitHere = 1
	}

	public Bully_Zone[] bullyZones;

	private Vector3 homePos = new Vector3(0f, -10f, 0f);

	private int zonesCount;

	private Bully_Zone currentZone;

	private int punishTime;

	private int changeClassTime;

	private IEnumerator punishCoroutine;

	private IEnumerator changeZoneCoroutine;

	private IEnumerator punishWaitCoroutine;

	protected override void Awake()
	{
		base.Awake();
		zonesCount = navMovePointsParent.childCount;
		bullyZones = new Bully_Zone[zonesCount];
		for (int i = 0; i < zonesCount; i++)
		{
			bullyZones[i] = navMovePointsParent.GetChild(i).GetChild(0).GetComponent<Bully_Zone>();
		}
		punishTime = GameplayManager.This.bully_punishTime;
		changeClassTime = GameplayManager.This.bully_changeClassTime;
		relaxTime = GameplayManager.This.bully_relaxTime;
		currentZone = bullyZones[0];
		npcSprite.SetActive(false);
		SetNewState(NpcState.MarkerMove);
		tform.position = homePos;
	}

	public override void EnterMarker(Collider other)
	{
		Sing_Game.This.playerController.EnableControlls(true);
		currentZone.EnableZone(false);
	}

	public override void PlayerAction()
	{
		if (ChangePlayerPunish(PunishChange.Check))
		{
			ChangePlayerPunish(PunishChange.Remove);
			npcSprite.SetActive(false);
			tform.position = homePos;
			Sing_Game.This.gameCanvas.PunishTimerEnable(false);
			currentZone.door.SetBlock(false);
			if (punishCoroutine != null)
			{
				StopCoroutine(punishCoroutine);
			}
			SetNewState(NpcState.MarkerMove);
		}
	}

	public override void PlayerAgro()
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
		PlaySound((BullySoundList)Random.Range(0, 2));
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
		currentZone.door.SetBlock(true);
		navMeshAgent.enabled = false;
		tform.position = currentZone.startPosiiton.position;
		navMeshAgent.enabled = true;
		npcSprite.SetActive(true);
		ActivateNav();
		SetTarget(currentZone.endPosition.position);
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
		yield return new WaitForSeconds(relaxTime);
		SetNewState(NpcState.MarkerMove);
	}

	protected override void UpdateState()
	{
		switch (npcState)
		{
		case NpcState.MarkerMove:
			EnableZones(true);
			break;
		case NpcState.PlayerPunish:
			EnableZones(false);
			PlayerPunish();
			break;
		}
	}

	public void EnableZones(bool isOn)
	{
		if (changeZoneCoroutine != null)
		{
			StopCoroutine(changeZoneCoroutine);
		}
		if (isOn)
		{
			changeZoneCoroutine = ChangeZone();
			StartCoroutine(changeZoneCoroutine);
		}
	}

	private IEnumerator ChangeZone()
	{
		while (true)
		{
			yield return new WaitForSeconds(changeClassTime);
			currentZone.EnableZone(false);
			currentZone = bullyZones[Random.Range(0, bullyZones.Length)];
			currentZone.EnableZone(true);
		}
	}

	private void PlaySound(BullySoundList _sound)
	{
		audioSourceMain.clip = audioStoryClips[(int)_sound];
		audioSourceMain.Play();
	}
}
