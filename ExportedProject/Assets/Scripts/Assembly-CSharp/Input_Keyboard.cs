using UnityEngine;

public class Input_Keyboard : MonoBehaviour, IGetInput
{
	public PlayerController_Main playerController;

	public Canvas_Main gameCanvas;

	public Input_Mobile input_Mobile;

	private Vector2 axisDirection = Vector2.zero;

	private float mouseAngle;

	private const int mouseK = 35;

	private bool isPause;

	private bool ifEndlessStamina;

	private void Start()
	{
		ifEndlessStamina = BaldinaShop.This.CheckBoost(BaldinaShop.BuyBoosts.EndlessStamina);
	}

	public void InitInput()
	{
		Object.Destroy(input_Mobile);
	}

	public void DisableRun()
	{
		if (!ifEndlessStamina)
		{
			playerController.EnableRun(false);
		}
	}

	public void GetInput()
	{
		axisDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		if (axisDirection != Vector2.zero)
		{
			playerController.AddMove(axisDirection);
		}
		else
		{
			playerController.AddForce();
		}
		mouseAngle = Input.GetAxis("Mouse X");
		if (mouseAngle != 0f)
		{
			playerController.AddRotation(mouseAngle * 35f);
		}
		if (Input.GetMouseButtonDown(0))
		{
			playerController.LeftClickAction();
		}
		if (Input.GetMouseButtonDown(1))
		{
			playerController.RightClickAction();
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			gameCanvas.NextChooseBoost(true);
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			gameCanvas.NextChooseBoost(false);
		}
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			playerController.EnableRun(true);
		}
		else if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			playerController.EnableRun(false);
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			playerController.LookBack(true);
		}
		else if (Input.GetKeyUp(KeyCode.Space))
		{
			playerController.LookBack(false);
		}
		if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
		{
		}
		if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
		{
		}
		if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
		{
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			isPause = !isPause;
			gameCanvas.SetPause(isPause);
		}
	}

	private void OnDestroy()
	{
	}
}
