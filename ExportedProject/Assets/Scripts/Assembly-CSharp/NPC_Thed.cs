using System.Collections.Generic;
using UnityEngine;

public class NPC_Thed : NPC_NavMove
{
	private enum ThedSoundList
	{
		Bulls = 0,
		Zone = 1,
		Steak = 2
	}

	private enum AnimState
	{
		Idle = 0,
		Rush = 1,
		WithBall = 2
	}

	public Boost ballBoost;

	public Boost goldBallBoost;

	public NPC_Main punishNpc;

	public NPC_Main punishNpcGold;

	public Transform playerRaycastEnd;

	private int moveSpeed;

	private int runSpeed;

	private float rayDistance;

	private int layerMaskPlayer = 8704;

	private int lastSound;

	protected override void Awake()
	{
		base.Awake();
		moveSpeed = GameplayManager.This.thed_navSpeed;
		runSpeed = GameplayManager.This.thed_navSpeedRun;
		rayDistance = npcZone.sphereCollider.radius;
		lastSound = Random.Range(0, 3);
		ActivateNav();
		SetNewState(NpcState.MarkerMove);
	}

	public void EnableZone(bool _isOn)
	{
		npcZone.EnableZone(_isOn);
		if (_isOn)
		{
			SetNewState(NpcState.MarkerMove);
		}
	}

	public override void ExitZone()
	{
		base.ExitZone();
		if (IsState(NpcState.PlayerRush))
		{
			SetNewState(NpcState.MarkerMove);
			spriteAnim.SetInteger("State", 0);
		}
	}

	public override void StayZone()
	{
		base.StayZone();
		if (!IsState(NpcState.PlayerRush))
		{
			CheckPlayer();
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

	public void CatchBall()
	{
		SetNewState(NpcState.BallRun);
	}

	public override void PlayerCollision()
	{
		List<NpcType> punishNpcType = Sing_Game.This.playerController.punishNpcType;
		if (punishNpcType.Contains(NpcType.Cleaner))
		{
			Sing_Game.This.npc_Cleaner.PlayerAction();
		}
		int num = lastSound;
		do
		{
			lastSound = Random.Range(0, 3);
		}
		while (num == lastSound);
		PlaySound((ThedSoundList)lastSound);
		ChangePlayerPunish(PunishChange.Add);
		Sing_Game.This.playerController.EnableControlls(false);
		Sing_Game.This.playerController.CameraFall(true);
		if (IsState(NpcState.PlayerRush))
		{
			SetNewState(NpcState.MarkerMove);
			npcZone.EnableZone(false);
			Sing_Game.This.gameCanvas.TakeBall();
			spriteAnim.SetInteger("State", 2);
			Invoke("DropBall", Random.Range(5, 6));
			Invoke("PunishEnd", GameplayManager.This.thed_punishTimeBall - 1);
		}
		else
		{
			Invoke("PunishEnd", GameplayManager.This.thed_punishTime - 1);
		}
	}

	private void PunishEnd()
	{
		ChangePlayerPunish(PunishChange.Remove);
		Sing_Game.This.playerController.CameraFall(false);
	}

	private void DropBall()
	{
		Sing_Game.This.ballFly.enabled = false;
		ballBoost.InitBoost();
		spriteAnim.SetInteger("State", 0);
	}

	public override void PlayerPunish()
	{
	}

	public override void PlayerAction()
	{
		CancelInvoke("PunishEnd");
		PunishEnd();
	}

	protected override void UpdateState()
	{
		switch (npcState)
		{
		case NpcState.MarkerMove:
			EnableFollow(false);
			navMeshAgent.speed = moveSpeed;
			SetRandomTarget();
			break;
		case NpcState.PlayerRush:
			spriteAnim.SetInteger("State", 1);
			EnableFollow(true, Sing_Game.This.playerController.tform);
			break;
		case NpcState.BallRun:
			navMeshAgent.speed = runSpeed;
			npcZone.EnableZone(false);
			if (punishNpcGold != null)
			{
				EnableFollow(true, goldBallBoost.tform);
			}
			else
			{
				EnableFollow(true, ballBoost.tform);
			}
			break;
		}
	}

	private void RelaxEnd()
	{
		npcZone.EnableZone(true);
		npcTrigger.EnableTrigger(true);
	}

	public override void BoostAction(BoostType _type)
	{
		base.BoostAction(_type);
		navMeshAgent.speed = moveSpeed;
		if (_type == BoostType.GoldBall)
		{
			goldBallBoost.UpdateParent();
			if (punishNpcGold != null)
			{
				punishNpcGold.ThedPunish();
				punishNpcGold = null;
			}
			if (punishNpc != null)
			{
				SetNewState(NpcState.BallRun);
			}
			else
			{
				SetNewState(NpcState.MarkerMove);
			}
		}
		else
		{
			ballBoost.UpdateParent();
			SetNewState(NpcState.MarkerMove);
			spriteAnim.SetInteger("State", 2);
			Invoke("DropBall", Random.Range(5, 6));
			if (punishNpc != null)
			{
				punishNpc.ThedPunish();
				punishNpc = null;
			}
		}
	}

	private void PlaySound(ThedSoundList _sound)
	{
		audioSourceMain.clip = audioStoryClips[(int)_sound];
		audioSourceMain.Play();
	}
}
