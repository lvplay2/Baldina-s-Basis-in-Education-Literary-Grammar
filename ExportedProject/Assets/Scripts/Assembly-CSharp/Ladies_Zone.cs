using UnityEngine;

public class Ladies_Zone : MonoBehaviour, IObjectArea
{
	public AudioSource audioSource;

	private bool isOnce = true;

	public void OnAreaEnter()
	{
		if (isOnce)
		{
			isOnce = false;
			audioSource.Play();
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
