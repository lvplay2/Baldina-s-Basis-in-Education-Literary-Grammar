using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Game : Canvas_Main
{
	public PlayerController_Game playerController;

	public Toggle runToggle;

	public Image runForceImage;

	public GameObject runForceObj;

	public GameObject punishTimerCanvas;

	public Text punishTimerText;

	private IEnumerator punishTimerCoroutine;

	public GameObject interfaceCanvas;

	public GameObject pauseCanvas;

	public Slider sliderSensitivity;

	public GameObject canvasSkip;

	public Text skipCountText;

	public Animator boostAnim;

	public Sprite[] boostSprites;

	public Image[] boostImages;

	public BoostType[] inventortBoosts = new BoostType[3];

	private int currentBoostChoosen;

	public Text boostNameText;

	public Text hintCountText;

	public GameObject bootsPlaceUse;

	public Image bootsPlaceUseImage;

	public bool isBoots;

	private int hintCount;

	private string[] boostNames = new string[12]
	{
		"Empty", "HINT", "BURGER", "BOOTS", "ANSWERS \"A+\"", "LOCK PICK", "NEWSPAPER", "BOOK", "KEY", "MIRROR",
		"Ball", "GoldBall"
	};

	public Door[] keyDoors;

	public List<PuzzleItems> puzzleItems = new List<PuzzleItems>();

	public GameObject puzzleCanvas;

	private void Start()
	{
		Door[] array = keyDoors;
		foreach (Door door in array)
		{
			door.SetBlock(true);
		}
		sliderSensitivity.value = GameplayManager.This.sensitivity;
		if (BaldinaShop.This.CheckBoost(BaldinaShop.BuyBoosts.EndlessStamina))
		{
			runForceObj.SetActive(false);
		}
		skipCountText.text = BaldinaShop.This.currentSkipCount.ToString();
	}

	public void ChangeSensitivity(float _value)
	{
		GameplayManager.This.sensitivity = _value;
		playerController.UpdateRotationSensitivity();
	}

	public void DisableRun()
	{
	}

	public void UpdateStamina(float value)
	{
		runForceImage.fillAmount = value;
	}

	public override void SetPause(bool _isOn)
	{
		if (_isOn)
		{
			interfaceCanvas.SetActive(false);
			pauseCanvas.SetActive(true);
			Time.timeScale = 0f;
			AudioListener.pause = true;
			if (Story_Game.This.storyNum > StoryEvent.EnterSchool || true)
			{
				BaldinaShop.This.UA_OpenGame();
			}
		}
		else
		{
			pauseCanvas.SetActive(false);
			interfaceCanvas.SetActive(true);
			Time.timeScale = 1f;
			AudioListener.pause = false;
			BaldinaShop.This.UA_OpenGame(false);
		}
		if (!Sing_Game.This.exampleTable.gobj.activeSelf)
		{
			GameplayManager.This.MY_SwitchCursor(_isOn);
		}
	}

	public override void AddBoost(BoostType _type)
	{
		if (_type == BoostType.Ball)
		{
			Sing_Game.This.npc_Thed.EnableZone(true);
		}
		if (_type == BoostType.Hint)
		{
			hintCount++;
			hintCountText.text = hintCount.ToString();
			Sing_Game.This.npc_Baldina.SpeedUp();
			if (hintCount == 8)
			{
				Story_Game.This.StoryTriggerEnter(StoryEvent.AllHintGet);
			}
			return;
		}
		int i;
		for (i = 0; i < 3; i++)
		{
			if (inventortBoosts[i] == BoostType.None)
			{
				inventortBoosts[i] = _type;
				ChangeBoostImage(i);
				break;
			}
		}
		if (i == 3)
		{
			inventortBoosts[currentBoostChoosen] = _type;
			ChangeBoostImage(currentBoostChoosen);
		}
		ChangeBoostName(currentBoostChoosen);
	}

	public void AddBoostShop(BoostType type, int placeId)
	{
		inventortBoosts[placeId] = type;
		RefreshBoost(placeId);
	}

	public override void NextChooseBoost(bool _next)
	{
		int num = currentBoostChoosen;
		if (_next)
		{
			num++;
			num = ((num <= 2) ? num : 0);
		}
		else
		{
			num--;
			num = ((num >= 0) ? num : 2);
		}
		ChooseBoost(num);
	}

	public override void ChooseBoost(int _id)
	{
		currentBoostChoosen = _id;
		boostAnim.SetInteger("ChooseId", currentBoostChoosen);
		ChangeBoostName(currentBoostChoosen);
	}

	private void RefreshBoost(int _id)
	{
		ChangeBoostName(_id);
		ChangeBoostImage(_id);
	}

	public override void ChangeBoostName(int _id)
	{
		boostNameText.text = boostNames[(int)inventortBoosts[_id]];
	}

	private void ChangeBoostImage(int _id)
	{
		boostImages[_id].sprite = boostSprites[(int)inventortBoosts[_id]];
	}

	public override void UseBoost()
	{
		switch (inventortBoosts[currentBoostChoosen])
		{
		case BoostType.Ball:
			Sing_Game.This.ballFly.InitFly(playerController.tform.position, playerController.tform.forward);
			Sing_Game.This.npc_Thed.CatchBall();
			inventortBoosts[currentBoostChoosen] = BoostType.None;
			RefreshBoost(currentBoostChoosen);
			break;
		case BoostType.GoldBall:
			Sing_Game.This.goldBallFly.InitFly(playerController.tform.position, playerController.tform.forward);
			inventortBoosts[currentBoostChoosen] = BoostType.None;
			RefreshBoost(currentBoostChoosen);
			break;
		case BoostType.Book:
			Sing_Game.This.boost_Book.PlaceBoost(playerController.tform.position);
			inventortBoosts[currentBoostChoosen] = BoostType.None;
			RefreshBoost(currentBoostChoosen);
			break;
		case BoostType.Boots:
			if (!playerController.ChangePunishNpcType(PunishChange.Check, NpcType.Cleaner))
			{
				StartCoroutine("BootsTick");
				inventortBoosts[currentBoostChoosen] = BoostType.None;
				RefreshBoost(currentBoostChoosen);
			}
			break;
		case BoostType.Burger:
			if (playerController.ChangePunishNpcType(PunishChange.Check, NpcType.Security))
			{
				Sing_Game.This.npc_Security.PlayerAction();
				inventortBoosts[currentBoostChoosen] = BoostType.None;
				RefreshBoost(currentBoostChoosen);
			}
			break;
		case BoostType.Examples:
			if (playerController.ChangePunishNpcType(PunishChange.Check, NpcType.Girl))
			{
				Sing_Game.This.npc_Girl.PlayerAction();
				inventortBoosts[currentBoostChoosen] = BoostType.None;
				RefreshBoost(currentBoostChoosen);
			}
			break;
		case BoostType.Key:
			if (playerController.keyArea)
			{
				Door[] array = keyDoors;
				foreach (Door door in array)
				{
					door.SetBlock(false);
				}
				inventortBoosts[currentBoostChoosen] = BoostType.None;
				RefreshBoost(currentBoostChoosen);
			}
			break;
		case BoostType.Mirror:
			if (Sing_Game.This.npc_Queen.CheckPlayer())
			{
				Sing_Game.This.npc_Queen.PlayerAction();
				inventortBoosts[currentBoostChoosen] = BoostType.None;
				RefreshBoost(currentBoostChoosen);
			}
			break;
		case BoostType.Newspaper:
			Sing_Game.This.boost_Newspaper.PlaceBoost(playerController.tform.position);
			inventortBoosts[currentBoostChoosen] = BoostType.None;
			RefreshBoost(currentBoostChoosen);
			break;
		case BoostType.Skelekey:
			if (playerController.ChangePunishNpcType(PunishChange.Check, NpcType.Bully))
			{
				Sing_Game.This.npc_Bully.PlayerAction();
				inventortBoosts[currentBoostChoosen] = BoostType.None;
				RefreshBoost(currentBoostChoosen);
			}
			break;
		}
	}

	private IEnumerator BootsTick()
	{
		isBoots = true;
		bootsPlaceUse.SetActive(true);
		float tick = 1f;
		float bootsTickSpeed = 0.5f / (float)GameplayManager.This.bootsTime;
		while (tick > 0f)
		{
			yield return new WaitForSeconds(0.5f);
			tick -= bootsTickSpeed;
			bootsPlaceUseImage.fillAmount = tick;
		}
		bootsPlaceUse.SetActive(false);
		isBoots = false;
		yield return null;
	}

	public override void PunishTimerEnable(bool isOn, int time = 0)
	{
		punishTimerCanvas.SetActive(isOn);
		if (punishTimerCoroutine != null)
		{
			StopCoroutine(punishTimerCoroutine);
		}
		if (isOn)
		{
			punishTimerCoroutine = PunishTimer(time);
			StartCoroutine(punishTimerCoroutine);
		}
	}

	private IEnumerator PunishTimer(int time)
	{
		int currentTimer = time;
		punishTimerText.text = currentTimer.ToString();
		while (currentTimer > 0)
		{
			yield return new WaitForSeconds(1f);
			currentTimer--;
			punishTimerText.text = currentTimer.ToString();
		}
		punishTimerCanvas.SetActive(false);
	}

	public void TakeBall()
	{
		for (int i = 0; i < 3; i++)
		{
			if (inventortBoosts[i] == BoostType.Ball)
			{
				inventortBoosts[i] = BoostType.None;
				RefreshBoost(i);
				break;
			}
		}
	}

	public void PuzzleStart()
	{
		puzzleCanvas.SetActive(true);
		GameplayManager.This.MY_SwitchCursor(true);
		int[] source = new int[8] { 350, 250, 150, 50, -50, -150, -250, -350 };
		System.Random random = new System.Random();
		source = source.OrderBy((int x) => random.Next()).ToArray();
		for (int i = 0; i < puzzleItems.Count; i++)
		{
			puzzleItems[i].InitPlace(source[i]);
		}
		Invoke("PuzzleEndTimer", GameplayManager.This.puzzleTime);
	}

	public void PuzzleEnd()
	{
		bool flag = true;
		for (int i = 0; i < puzzleItems.Count; i++)
		{
			if (!puzzleItems[i].inPlace)
			{
				return;
			}
		}
		for (int j = 0; j < puzzleItems.Count; j++)
		{
			if (!puzzleItems[j].isCorrect)
			{
				flag = false;
				break;
			}
		}
		puzzleCanvas.SetActive(false);
		GameplayManager.This.MY_SwitchCursor(false);
		if (flag)
		{
			MultiSceneManager.This.LoadScene("Win");
		}
		else
		{
			Sing_Game.This.npc_Baldina.PlayerCollision();
		}
	}

	private void PuzzleEndTimer()
	{
		puzzleCanvas.SetActive(false);
		GameplayManager.This.MY_SwitchCursor(false);
		Sing_Game.This.npc_Baldina.PlayerCollision();
	}

	public void EnableSkipButton(bool isOn = true)
	{
		canvasSkip.SetActive(isOn);
	}

	public void UA_SkipPunishes()
	{
		if (BaldinaShop.This.currentSkipCount > 0 && playerController.punishNpcType.Count > 0)
		{
			BaldinaShop.This.currentSkipCount--;
			skipCountText.text = BaldinaShop.This.currentSkipCount.ToString();
			playerController.SkipPunishes();
		}
	}
}
