using UnityEngine;

public class StoryTriggers : MonoBehaviour, IObjectArea
{
	public StoryEvent storyEvent;

	public void OnAreaEnter()
	{
		Story_Game.This.StoryTriggerEnter(storyEvent);
	}

	public void OnAreaEnter_Npc()
	{
	}

	public void OnAreaExit()
	{
	}

	public void OnAreaExit_Npc()
	{
	}
}
