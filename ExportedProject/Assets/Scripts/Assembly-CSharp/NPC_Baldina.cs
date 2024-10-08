using System.Collections;
using UnityEngine;

public class NPC_Baldina : NPC_NavMove
{
	private enum AnimState
	{
		Stay = 0,
		Talk = 1,
		Walk = 2,
		Run = 3,
		Read = 4
	}

	private enum BaldinaSoundList
	{
		Welcome = 0,
		ClassRoom = 1,
		ImGoingToYou = 2,
		HereYouAre = 3,
		RecitePoem = 4
	}

	public Transform classPosition;

	public AudioClip[] audioClips;

	public AudioSource audioSourceLeg;

	public AudioClip[] audioClipLeg;

	public Transform playerTform;

	public ScreamerRotation screamerComponent;

	private float stepDelayDecrease;

	private float navMeshSpeed;

	private float navMeshStepTime;

	public float navMeshStepDelay;

	private bool isLeftLeg;

	protected override void Awake()
	{
		base.Awake();
		npcType = NpcType.Baldina;
		npcTrigger.EnableTrigger(false);
		if (BaldinaShop.This.CheckBoost(BaldinaShop.BuyBoosts.SlowBaldina))
		{
			navMeshSpeed = GameplayManager.This.baldina_NavMeshSpeedSlow;
		}
		else
		{
			navMeshSpeed = GameplayManager.This.baldina_NavMeshSpeed;
		}
		navMeshStepTime = GameplayManager.This.baldina_StepTime;
		navMeshStepDelay = GameplayManager.This.baldina_MinMaxStepDelay.y;
		stepDelayDecrease = (GameplayManager.This.baldina_MinMaxStepDelay.y - GameplayManager.This.baldina_MinMaxStepDelay.x) / 8f;
		StartAudioRandom(false);
	}

	public void Welcome()
	{
		StartCoroutine("Welcoming");
	}

	private IEnumerator Welcoming()
	{
		Sing_Game.This.playerController.EnableControlls(false);
		PlaySound(BaldinaSoundList.Welcome);
		spriteAnim.SetInteger("State", 1);
		yield return new WaitForSeconds(6f);
		Sing_Game.This.playerController.EnableControlls(true);
		spriteAnim.SetInteger("State", 2);
		ActivateNav();
		GoToClass();
		while (Vector2.Distance(tform.position, classPosition.position) > 0.1f)
		{
			yield return new WaitForSeconds(0.1f);
		}
		while (Story_Game.This.storyNum < StoryEvent.EnterClass)
		{
			yield return new WaitForSeconds(1f);
		}
		ActivateNav(false);
		tform.position = classPosition.position;
		PlaySound(BaldinaSoundList.ClassRoom);
		spriteAnim.SetInteger("State", 1);
		yield return new WaitForSeconds(15f);
		spriteAnim.SetInteger("State", 0);
		yield return new WaitForSeconds(GameplayManager.This.baldina_BreakTime);
		PlaySound(BaldinaSoundList.ImGoingToYou);
		spriteAnim.SetInteger("State", 3);
		ActivateNav();
		npcTrigger.EnableTrigger(true);
		Story_Game.This.StartFastTheme();
		StartCoroutine("PlayerRush");
		StartAudioRandom();
	}

	public void SpeedUp()
	{
		navMeshStepDelay -= stepDelayDecrease;
	}

	private IEnumerator PlayerRush()
	{
		npcTrigger.EnableTrigger(true);
		while (true)
		{
			SetTarget(playerTform.position);
			navMeshAgent.speed = navMeshSpeed;
			spriteAnim.SetBool("LeftLeg", isLeftLeg);
			if (isLeftLeg)
			{
				isLeftLeg = false;
				PlayLeg(0);
			}
			else
			{
				isLeftLeg = true;
				PlayLeg(1);
			}
			yield return new WaitForSeconds(navMeshStepTime);
			navMeshAgent.speed = 0f;
			yield return new WaitForSeconds(navMeshStepDelay);
		}
	}

	private void PlayLeg(int _num)
	{
		audioSourceLeg.clip = audioClipLeg[_num];
		audioSourceLeg.Play();
	}

	private void PlaySound(BaldinaSoundList _sound)
	{
		audioSourceMain.clip = audioClips[(int)_sound];
		audioSourceMain.Play();
	}

	public void GoToClass(bool _isTeleport = false)
	{
		if (_isTeleport)
		{
			StartAudioRandom(false);
			StopCoroutine("PlayerRush");
			ActivateNav(false);
			StartCoroutine("PuzzleStarting");
			tform.position = classPosition.position;
		}
		else
		{
			SetTarget(classPosition.position);
		}
	}

	private IEnumerator PuzzleStarting()
	{
		spriteAnim.SetInteger("State", 1);
		PlaySound(BaldinaSoundList.HereYouAre);
		yield return new WaitForSeconds(3f);
		spriteAnim.SetInteger("State", 0);
	}

	public override void PlayerCollision()
	{
		screamerComponent.enabled = true;
		Sing_Game.This.playerController.EnableControlls(false);
	}

	public override void PlayerAction()
	{
		base.PlayerAction();
	}

	public override void BoostAction(BoostType _type)
	{
		spriteAnim.SetInteger("State", 4);
		ActivateNav(false);
		npcTrigger.EnableTrigger(false);
		StopCoroutine("PlayerRush");
		Invoke("BoostEnd", GameplayManager.This.baldina_BookTime);
	}

	private void BoostEnd()
	{
		ActivateNav();
		spriteAnim.SetInteger("State", 3);
		npcTrigger.EnableTrigger(true);
		StartCoroutine("PlayerRush");
	}
}
