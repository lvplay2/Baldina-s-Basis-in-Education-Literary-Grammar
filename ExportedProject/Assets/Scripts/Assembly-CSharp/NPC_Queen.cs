using System.Collections.Generic;
using UnityEngine;

public class NPC_Queen : NPC_NavMove
{
	private enum QueenSoundList
	{
		NiceTrick = 0,
		Ooh = 1
	}

	public Transform playerRaycastEnd;

	private PlayerController_Game playerController;

	private float rayDistance;

	private int layerMaskPlayer = 8704;

	private int stoneTime;

	protected override void Awake()
	{
		base.Awake();
		rayDistance = npcZone.sphereCollider.radius;
		playerController = Sing_Game.This.playerController;
		stoneTime = GameplayManager.This.queen_stoneTime;
		ActivateNav();
		SetNewState(NpcState.MarkerMove);
		StartAudioRandom();
	}

	public override void StayZone()
	{
		List<NpcType> punishNpcType = Sing_Game.This.playerController.punishNpcType;
		if (!punishNpcType.Contains(NpcType.Girl) && !punishNpcType.Contains(NpcType.Thed) && CheckPlayer())
		{
			playerController.QueenRotation(tform.position);
		}
	}

	public override void PlayerAction()
	{
		PlaySound((QueenSoundList)Random.Range(0, 2));
		npcZone.EnableZone(false);
		ActivateNav(false);
		spriteAnim.SetBool("Stone", true);
		Invoke("StoneEnd", stoneTime);
	}

	private void StoneEnd()
	{
		if (gobj.activeSelf)
		{
			npcZone.EnableZone(true);
			ActivateNav();
			spriteAnim.SetBool("Stone", false);
			SetNewState(NpcState.MarkerMove);
		}
	}

	public bool CheckPlayer()
	{
		Vector3 vector = playerRaycastEnd.position - raycastStart.position;
		Vector3 direction = vector / vector.magnitude;
		RaycastHit hitInfo;
		if (Physics.Raycast(tform.position, direction, out hitInfo, rayDistance, layerMaskPlayer) && hitInfo.transform.tag == "Player")
		{
			return true;
		}
		return false;
	}

	protected override void UpdateState()
	{
		NpcState npcState = base.npcState;
		if (npcState == NpcState.MarkerMove)
		{
			SetRandomTarget();
		}
	}

	private void PlaySound(QueenSoundList _sound)
	{
		StartAudioRandom(false);
		audioSourceMain.clip = audioStoryClips[(int)_sound];
		audioSourceMain.Play();
		StartAudioRandom();
	}
}
