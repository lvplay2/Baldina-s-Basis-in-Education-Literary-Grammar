using System.Collections.Generic;
using UnityEngine;

public class Story_Game : MonoBehaviour
{
	public AudioSource backgroundMusic;

	public AudioClip backgroundAudioClip;

	public Transform storyTriggersParent;

	public Door[] startClosingDoors;

	private GameObject[] storyTriggers;

	public StoryEvent storyNum;

	public GameObject boosts;

	public int randomPuzzleText;

	public List<string[]> puzzleTexts = new List<string[]>
	{
		new string[8] { "We don't need no education", "We don't need no thought control", "No dark sarcasm in the classroom", "Teachers leave them kids alone", "Hey! Teachers! Leave them kids alone", "All in all it's just another brick in the wall", "All in all you're just", "Another brick in the wall" },
		new string[8] { "Oh the weather outside is frightful", "But the fire is so delightful", "Since we've no place to go", "Let it snow, let it snow, let it snow,", "It doesn't show signs of stopping", "And I've brought some corn for popping", "The lights are turned down low", "Let it snow, let it snow, let it snow!" }
	};

	public static Story_Game This { get; private set; }

	private void Awake()
	{
		This = this;
		int childCount = storyTriggersParent.childCount;
		storyTriggers = new GameObject[childCount];
		for (int i = 0; i < childCount; i++)
		{
			storyTriggers[i] = storyTriggersParent.GetChild(i).gameObject;
		}
		StartDoorsClose(true);
		randomPuzzleText = Random.Range(0, puzzleTexts.Count);
	}

	private void StartDoorsClose(bool _isOn)
	{
		for (int i = 0; i < startClosingDoors.Length; i++)
		{
			startClosingDoors[i].SetBlock(_isOn);
		}
	}

	public void GoMainMenu()
	{
		MultiSceneManager.This.LoadScene("Menu");
	}

	public void StoryTriggerEnter(StoryEvent _event)
	{
		storyNum = _event;
		switch (_event)
		{
		case StoryEvent.EnterSchool:
			storyTriggers[0].SetActive(false);
			storyTriggers[1].SetActive(true);
			Sing_Game.This.npc_Baldina.Welcome();
			break;
		case StoryEvent.EnterClass:
			StartDoorsClose(false);
			storyTriggers[1].SetActive(false);
			boosts.SetActive(true);
			Sing_Game.This.EnableNpc();
			break;
		case StoryEvent.AllHintGet:
			storyTriggers[2].SetActive(true);
			break;
		case StoryEvent.PuzzleStart:
			storyTriggers[2].SetActive(false);
			Sing_Game.This.playerController.EnableControlls(false);
			Sing_Game.This.npc_Baldina.GoToClass(true);
			Sing_Game.This.gameCanvas.PuzzleStart();
			Sing_Game.This.EnableNpc(false);
			break;
		}
	}

	public void StartFastTheme()
	{
		backgroundMusic.clip = backgroundAudioClip;
		backgroundMusic.loop = false;
		backgroundMusic.Play();
	}
}
