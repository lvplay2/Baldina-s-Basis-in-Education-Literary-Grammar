using UnityEngine;

public class Key_Zone : MonoBehaviour, IObjectArea
{
	public void OnAreaEnter()
	{
		Sing_Game.This.playerController.keyArea = true;
	}

	public void OnAreaEnter_Npc()
	{
	}

	public void OnAreaExit()
	{
		Sing_Game.This.playerController.keyArea = false;
	}

	public void OnAreaExit_Npc()
	{
	}
}
