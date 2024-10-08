using System.Collections.Generic;
using UnityEngine;

public class NPC_Director : NPC_NavMove
{
	private enum DirectorSoundList
	{
		RunCoridor = 0,
		Wooow = 1
	}

	public Transform punishClass;

	public Door[] punishDoors;

	public AudioSource classRoomAudioSource;

	public Transform playerRaycastEnd;

	private CharacterController playerCharContr;

	private int punishTime;

	private float rayDistance;

	private int layerMaskPlayer = 8704;

	private bool isWarning;

	protected override void Awake()
	{
		base.Awake();
		rayDistance = npcZone.sphereCollider.radius;
		playerCharContr = Sing_Game.This.playerController.charContr;
		punishTime = GameplayManager.This.director_punishTime;
		ActivateNav();
		SetNewState(NpcState.MarkerMove);
		StartAudioRandom();
	}

	public override void EnterZone()
	{
		base.EnterZone();
	}

	public override void StayZone()
	{
		base.StayZone();
		if (!IsState(NpcState.PlayerRush) && playerCharContr.velocity != Vector3.zero)
		{
			CheckPlayer();
		}
	}

	public override void ExitZone()
	{
		base.ExitZone();
	}

	private void CheckPlayer()
	{
		Vector3 vector = playerRaycastEnd.position - raycastStart.position;
		Vector3 direction = vector / vector.magnitude;
		RaycastHit hitInfo;
		if (Physics.Raycast(tform.position, direction, out hitInfo, rayDistance, layerMaskPlayer) && hitInfo.transform.tag == "Player")
		{
			if (isWarning)
			{
				SetNewState(NpcState.PlayerRush);
				return;
			}
			isWarning = true;
			PlaySound(DirectorSoundList.RunCoridor);
			Sing_Game.This.gameCanvas.DisableRun();
			EnableZone(false);
		}
	}

	public override void PlayerCollision()
	{
		if (!IsState(NpcState.Relax))
		{
			SetNewState(NpcState.PlayerPunish);
		}
	}

	public override void PlayerPunish()
	{
		List<NpcType> punishNpcType = Sing_Game.This.playerController.punishNpcType;
		if (punishNpcType.Contains(NpcType.Girl))
		{
			Sing_Game.This.npc_Girl.PlayerAction();
		}
		if (punishNpcType.Contains(NpcType.Cleaner))
		{
			Sing_Game.This.npc_Cleaner.PlayerAction();
		}
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
		Sing_Game.This.playerController.tform.position = punishClass.position;
		Door[] array = punishDoors;
		foreach (Door door in array)
		{
			door.SetBlock(true);
		}
		classRoomAudioSource.Play();
		Sing_Game.This.gameCanvas.PunishTimerEnable(true, punishTime);
		Invoke("PunishEnd", punishTime);
		SetNewState(NpcState.MarkerMove);
	}

	private void PunishEnd()
	{
		ChangePlayerPunish(PunishChange.Remove);
		Door[] array = punishDoors;
		foreach (Door door in array)
		{
			door.SetBlock(false);
		}
		Sing_Game.This.gameCanvas.PunishTimerEnable(false);
	}

	public override void PlayerAction()
	{
		CancelInvoke("PunishEnd");
		PunishEnd();
	}

	public void EnableZone(bool _isOn)
	{
		if (!IsState(NpcState.PlayerRush))
		{
			npcZone.EnableZone(_isOn);
		}
	}

	protected override void UpdateState()
	{
		switch (npcState)
		{
		case NpcState.PlayerRush:
			npcZone.EnableZone(false);
			npcTrigger.EnableTrigger(true);
			EnableFollow(true, Sing_Game.This.playerController.tform);
			break;
		case NpcState.PlayerPunish:
			npcTrigger.EnableTrigger(false);
			PlayerPunish();
			break;
		case NpcState.MarkerMove:
			npcTrigger.EnableTrigger(false);
			EnableFollow(false);
			SetRandomTarget();
			break;
		}
	}

	public override void BoostAction(BoostType _type)
	{
		PlaySound(DirectorSoundList.Wooow);
		ActivateNav(false);
		npcTrigger.EnableTrigger(false);
		Invoke("BoostEnd", GameplayManager.This.director_NewspaperTime);
	}

	private void BoostEnd()
	{
		ActivateNav();
		SetNewState(NpcState.MarkerMove);
		npcTrigger.EnableTrigger(true);
	}

	private void PlaySound(DirectorSoundList _sound)
	{
		audioSourceMain.clip = audioStoryClips[(int)_sound];
		audioSourceMain.Play();
	}
}
