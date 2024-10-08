using UnityEngine;

public class Debug_Temp : MonoBehaviour
{
	private bool isController;

	public static Debug_Temp This { get; private set; }

	private void Awake()
	{
		This = this;
	}

	private void Start()
	{
	}
}
