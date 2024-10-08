using System.Collections.Generic;
using UnityEngine;

public class NPC_Girl : NPC_NavMove
{
	private enum GirlSoundList
	{
		SolveIt = 0,
		SayAnswer = 1,
		Woowt = 2,
		Laught = 3
	}

	public ExamplesTable examplesTable;

	public Transform playerRaycastEnd;

	private float rayDistance;

	private int layerMaskPlayer = 8704;

	protected override void Awake()
	{
		base.Awake();
		relaxTime = GameplayManager.This.girl_relaxTime;
		rayDistance = npcZone.sphereCollider.radius;
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
		if (!IsState(NpcState.PlayerRush))
		{
			CheckPlayer();
		}
	}

	public override void ExitZone()
	{
		base.ExitZone();
		if (IsState(NpcState.PlayerRush))
		{
			SetNewState(NpcState.MarkerMove);
		}
	}

	private void CheckPlayer()
	{
		Vector3 vector = playerRaycastEnd.position - raycastStart.position;
		Vector3 direction = vector / vector.magnitude;
		RaycastHit hitInfo;
		if (Physics.Raycast(tform.position, direction, out hitInfo, rayDistance, layerMaskPlayer) && hitInfo.transform.tag == "Player")
		{
			SetNewState(NpcState.PlayerRush);
		}
	}

	public override void PlayerCollision()
	{
		SetNewState(NpcState.PlayerPunish);
	}

	public override void PlayerPunish()
	{
		List<NpcType> punishNpcType = Sing_Game.This.playerController.punishNpcType;
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
		Sing_Game.This.playerController.EnableControlls(false);
		Sing_Game.This.playerController.EnableRotation(false);
		examplesTable.EnableTable(true);
		PlaySound((GirlSoundList)Random.Range(0, 2));
	}

	public override void PlayerAction()
	{
		if (ChangePlayerPunish(PunishChange.Check))
		{
			ChangePlayerPunish(PunishChange.Remove);
			examplesTable.EnableTable(false);
			Sing_Game.This.playerController.EnableRotation(true);
			Sing_Game.This.playerController.EnableControlls(true);
			SetNewState(NpcState.MarkerMove);
			Invoke("RelaxEnd", relaxTime);
			PlaySound((GirlSoundList)Random.Range(2, 4));
		}
	}

	protected override void UpdateState()
	{
		switch (npcState)
		{
		case NpcState.MarkerMove:
			ActivateNav();
			EnableFollow(false);
			SetRandomTarget();
			break;
		case NpcState.PlayerRush:
			EnableFollow(true, Sing_Game.This.playerController.tform);
			break;
		case NpcState.PlayerPunish:
			EnableFollow(false);
			npcZone.EnableZone(false);
			npcTrigger.EnableTrigger(false);
			ActivateNav(false);
			PlayerPunish();
			break;
		}
	}

	private void RelaxEnd()
	{
		if (gobj.activeSelf)
		{
			npcZone.EnableZone(true);
			npcTrigger.EnableTrigger(true);
		}
	}

	private void PlaySound(GirlSoundList _sound)
	{
		audioSourceMain.clip = audioStoryClips[(int)_sound];
		audioSourceMain.Play();
	}
}
