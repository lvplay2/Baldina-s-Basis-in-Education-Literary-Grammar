using UnityEngine;
using UnityEngine.UI;

public class Input_Mobile : MonoBehaviour, IGetInput
{
	public HiddenJoystickPanel moveJoystick;

	public RotateTouch rotateTouch;

	public PlayerController_Main playerController;

	public Input_Keyboard input_Keyboard;

	public GameObject go_TouchInterface;

	public Toggle runToggle;

	private bool ifEndlessStamina;

	private void Start()
	{
		ifEndlessStamina = BaldinaShop.This.CheckBoost(BaldinaShop.BuyBoosts.EndlessStamina);
	}

	public void InitInput()
	{
		Object.Destroy(input_Keyboard);
	}

	public void DisableRun()
	{
		if (!ifEndlessStamina)
		{
			runToggle.isOn = false;
		}
	}

	public void GetInput()
	{
		if (moveJoystick.isDrag)
		{
			playerController.AddMove(moveJoystick.GetDirection());
		}
		else
		{
			playerController.AddForce();
		}
		if (rotateTouch.isDrag)
		{
			playerController.AddRotation(rotateTouch.GetAngle().x);
		}
	}

	private void OnDestroy()
	{
		Object.Destroy(go_TouchInterface);
	}
}
