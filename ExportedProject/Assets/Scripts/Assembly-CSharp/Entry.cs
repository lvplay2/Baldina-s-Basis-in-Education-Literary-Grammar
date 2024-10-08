using UnityEngine;

public class Entry : MonoBehaviour
{
	private void Start()
	{
		MultiSceneManager.This.LoadScene("Tutorial");
	}
}
