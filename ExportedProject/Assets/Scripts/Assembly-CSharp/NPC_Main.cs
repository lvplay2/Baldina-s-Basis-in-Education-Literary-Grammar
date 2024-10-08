using System.Linq;
using UnityEngine;

public class NPC_Main : MonoBehaviour
{
	protected enum NpcState
	{
		None = 0,
		MarkerMove = 1,
		PlayerRush = 2,
		PlayerPunish = 3,
		Relax = 4,
		Freeze = 5,
		BallRun = 6
	}

	public GameObject gobj;

	public Transform tform;

	public Animator spriteAnim;

	public GameObject npcSprite;

	public NPC_Trigger npcTrigger;

	public NPC_Zone npcZone;

	public NPC_NavMove_Callback npcMarkerCallback;

	public NpcType npcType;

	public int relaxTime;

	public BoostType[] effectBoosts;

	public Vector2 minMaxAudioRandomTime;

	public AudioSource audioSourceMain;

	public AudioClip[] audioRandomClips;

	public AudioClip[] audioStoryClips;

	private int audioRandomClipCount;

	protected NpcState npcState;

	protected virtual void Awake()
	{
		if (gobj == null)
		{
			gobj = base.gameObject;
			Debug.LogWarning("Set (gobj) variable on " + gobj.name);
		}
		if (tform == null)
		{
			tform = base.transform;
			Debug.LogWarning("Set (tform) variable on " + gobj.name);
		}
		audioRandomClipCount = audioRandomClips.Length;
	}

	protected void StartAudioRandom(bool isOn = true)
	{
		if (isOn)
		{
			if (audioRandomClipCount > 0)
			{
				InvokeRepeating("AudioRandom", Random.Range(minMaxAudioRandomTime.x, minMaxAudioRandomTime.y), Random.Range(minMaxAudioRandomTime.x, minMaxAudioRandomTime.y));
			}
		}
		else
		{
			CancelInvoke("AudioRandom");
		}
	}

	private void AudioRandom()
	{
		audioSourceMain.clip = audioRandomClips[Random.Range(0, audioRandomClipCount)];
		audioSourceMain.Play();
	}

	public virtual void PlayerAgro()
	{
	}

	public virtual void PlayerPunish()
	{
	}

	public virtual void PlayerAction()
	{
	}

	public virtual void PlayerCollision()
	{
	}

	public virtual void BoostAction(BoostType _type)
	{
	}

	public bool CheckBoostEffectType(BoostType _type)
	{
		return effectBoosts.Contains(_type);
	}

	public virtual void EnterZone()
	{
	}

	public virtual void StayZone()
	{
	}

	public virtual void ExitZone()
	{
	}

	protected virtual void UpdateState()
	{
	}

	protected void SetNewState(NpcState _state)
	{
		if (gobj.activeSelf)
		{
			npcState = _state;
			UpdateState();
		}
	}

	protected bool IsState(NpcState _state)
	{
		return npcState == _state;
	}

	protected bool ChangePlayerPunish(PunishChange _command)
	{
		return Sing_Game.This.playerController.ChangePunishNpcType(_command, npcType);
	}

	public virtual void ThedPunish()
	{
	}

	public virtual void DisableInGame()
	{
		PlayerAction();
		StartAudioRandom(false);
		gobj.SetActive(false);
		if ((bool)tform.Find("Ball"))
		{
			Sing_Game.This.npc_Thed.BoostAction(BoostType.Ball);
		}
	}
}
