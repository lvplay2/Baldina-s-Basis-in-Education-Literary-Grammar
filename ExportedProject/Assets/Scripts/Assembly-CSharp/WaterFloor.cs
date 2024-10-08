using UnityEngine;

public class WaterFloor : MonoBehaviour, IObjectArea
{
	public GameObject gobj;

	private void Awake()
	{
	}

	public void OnEnable()
	{
	}

	private void DisableWater()
	{
		gobj.SetActive(false);
	}

	public void OnAreaEnter()
	{
		if (!Sing_Game.This.gameCanvas.isBoots)
		{
			Sing_Game.This.npc_Cleaner.PlayerPunish();
		}
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
