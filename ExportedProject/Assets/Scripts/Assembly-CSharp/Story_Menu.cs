using UnityEngine;

public class Story_Menu : MonoBehaviour
{
	public static Story_Menu This { get; private set; }

	private void Awake()
	{
		This = this;
	}
}
