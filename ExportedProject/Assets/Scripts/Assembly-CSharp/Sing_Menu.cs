using UnityEngine;

public class Sing_Menu : MonoBehaviour
{
	public Canvas_Menu canvasMenu;

	public static Sing_Menu This { get; private set; }

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
