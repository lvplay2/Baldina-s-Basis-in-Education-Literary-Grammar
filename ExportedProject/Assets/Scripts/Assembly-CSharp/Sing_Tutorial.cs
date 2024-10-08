using UnityEngine;

public class Sing_Tutorial : MonoBehaviour
{
	public static Sing_Tutorial This { get; private set; }

	private void Awake()
	{
		This = this;
		if (!GameObject.Find("MultiSceneManager"))
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("Prefabs/MultiSceneManager") as GameObject);
			gameObject.name = "MultiSceneManager";
		}
	}
}
