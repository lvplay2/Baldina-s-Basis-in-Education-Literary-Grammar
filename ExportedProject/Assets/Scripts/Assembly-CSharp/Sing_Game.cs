using UnityEngine;

public class Sing_Game : MonoBehaviour
{
	public IGetInput currentPlatformInput;

	public PlayerController_Game playerController;

	public Canvas_Game gameCanvas;

	public Transform birds;

	public ExamplesTable exampleTable;

	public NPC_Main[] npcCharacters;

	public NPC_Baldina npc_Baldina;

	public NPC_Girl npc_Girl;

	public NPC_Director npc_Director;

	public NPC_Cleaner npc_Cleaner;

	public NPC_Bully npc_Bully;

	public NPC_Security npc_Security;

	public NPC_Queen npc_Queen;

	public NPC_Thed npc_Thed;

	public Boost boost_Book;

	public Boost boost_Newspaper;

	public BallFly ballFly;

	public GoldBallFly goldBallFly;

	public static Sing_Game This { get; private set; }

	private void Awake()
	{
		This = this;
		if (!GameObject.Find("MultiSceneManager"))
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("Prefabs/MultiSceneManager") as GameObject);
			gameObject.name = "MultiSceneManager";
		}
		if (GameplayManager.This.currentDevice == GameplayManager.DeviceType.Mobile)
		{
			currentPlatformInput = GetComponent<Input_Mobile>();
		}
		else
		{
			currentPlatformInput = GetComponent<Input_Keyboard>();
		}
		npcCharacters = new NPC_Main[7] { npc_Girl, npc_Director, npc_Cleaner, npc_Bully, npc_Security, npc_Queen, npc_Thed };
		GameplayManager.This.MY_SwitchCursor(false);
	}

	private void Update()
	{
		currentPlatformInput.GetInput();
	}

	public void DisableRun()
	{
		currentPlatformInput.DisableRun();
	}

	public void EnableNpc(bool isOn = true)
	{
		for (int i = 0; i < 7; i++)
		{
			npcCharacters[i].gobj.SetActive(isOn);
		}
	}
}
