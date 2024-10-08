using UnityEngine;

public class Bully_Zone : MonoBehaviour, IObjectArea
{
	public GameObject gobj;

	public Transform startPosiiton;

	public Transform endPosition;

	public Door door;

	public void EnableZone(bool isOn)
	{
		gobj.SetActive(isOn);
	}

	public void OnAreaEnter()
	{
		Sing_Game.This.npc_Bully.PlayerAgro();
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
