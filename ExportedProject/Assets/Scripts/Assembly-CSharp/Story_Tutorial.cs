using UnityEngine;

public class Story_Tutorial : MonoBehaviour
{
	public static Story_Tutorial This { get; private set; }

	private void Awake()
	{
		This = this;
	}
}
