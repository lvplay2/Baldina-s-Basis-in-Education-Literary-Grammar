using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Main : MonoBehaviour
{
	public Transform tform;

	public Transform camTform;

	public Canvas_Game gameCanvas;

	public Camera cam;

	public CharacterController charContr;

	public float takeDistance;

	public List<NpcType> punishNpcType = new List<NpcType>();

	private Vector3 cameraDirectionBack = new Vector3(0f, 180f, 0f);

	private float currentMoveSpeed;

	private float currentRotateSpeed;

	private bool isFreeze;

	private bool isRun;

	private float runForce = 1f;

	private float runForceIncreaseSpeed = 0.2f;

	private float runForceDecreaseSpeed = 0.2f;

	public virtual void Awake()
	{
		currentMoveSpeed = GameplayManager.This.playerMoveSpeed;
		UpdateRotationSensitivity();
		Debug.Log(currentRotateSpeed);
	}

	public virtual void AddMove(Vector2 _direction)
	{
		charContr.Move((_direction.y * tform.forward + _direction.x * tform.right) * currentMoveSpeed * Time.deltaTime);
		if (isRun)
		{
			runForce -= Time.deltaTime * runForceDecreaseSpeed;
			if (runForce < 0f)
			{
				runForce = 0f;
				Sing_Game.This.DisableRun();
			}
			gameCanvas.UpdateStamina(runForce);
		}
	}

	public virtual void UpdateRotationSensitivity()
	{
		currentRotateSpeed = GameplayManager.This.sensitivity;
		Debug.Log(currentRotateSpeed);
	}

	public virtual void AddMove(Vector3 _direction)
	{
		charContr.Move(_direction * currentMoveSpeed * Time.deltaTime);
		if (isRun)
		{
			runForce -= Time.deltaTime * runForceDecreaseSpeed;
			if (runForce < 0f)
			{
				runForce = 0f;
				Sing_Game.This.DisableRun();
			}
			gameCanvas.UpdateStamina(runForce);
		}
	}

	public virtual void AddForce()
	{
		runForce += Time.deltaTime * runForceIncreaseSpeed;
		if (runForce > 1f)
		{
			runForce = 1f;
		}
		gameCanvas.UpdateStamina(runForce);
	}

	public virtual void AddRotation(Vector2 _axis)
	{
	}

	public virtual void AddRotation(float _angle)
	{
		tform.Rotate(0f, _angle * currentRotateSpeed * Time.deltaTime, 0f);
	}

	public virtual void LookBack(bool _isOn)
	{
		Debug.Log(_isOn);
		if (_isOn)
		{
			camTform.localEulerAngles = cameraDirectionBack;
		}
		else
		{
			camTform.localEulerAngles = Vector3.zero;
		}
	}

	public virtual void LeftClickAction()
	{
	}

	public virtual void RightClickAction()
	{
	}

	public virtual void ActionClick()
	{
	}

	public virtual void EnableRun(bool _isOn)
	{
		Vector3 position = tform.position;
		position.y = 0.14f;
		tform.position = position;
		if (!isFreeze)
		{
			isRun = _isOn;
			Sing_Game.This.npc_Director.EnableZone(isRun);
			if (_isOn)
			{
				currentMoveSpeed = GameplayManager.This.playerRunSpeed;
			}
			else
			{
				currentMoveSpeed = GameplayManager.This.playerMoveSpeed;
			}
		}
	}

	public virtual void EnableControlls(bool _isOn)
	{
		isFreeze = !_isOn;
		if (isFreeze)
		{
			isRun = false;
			currentMoveSpeed = 0f;
			Sing_Game.This.npc_Director.EnableZone(false);
			Sing_Game.This.DisableRun();
		}
		else if (!ChangePunishNpcType(PunishChange.Check, NpcType.Girl) && !ChangePunishNpcType(PunishChange.Check, NpcType.Cleaner) && !ChangePunishNpcType(PunishChange.Check, NpcType.Security) && !ChangePunishNpcType(PunishChange.Check, NpcType.Thed))
		{
			EnableRun(isRun);
		}
	}

	public virtual void EnableRotation(bool isOn)
	{
		if (isOn)
		{
			currentRotateSpeed = GameplayManager.This.sensitivity;
		}
		else
		{
			currentRotateSpeed = 0f;
		}
	}

	public virtual void OnTriggerEnter(Collider other)
	{
	}

	public virtual void OnTriggerStay(Collider other)
	{
	}

	public virtual void OnTriggerExit(Collider other)
	{
	}

	public bool ChangePunishNpcType(PunishChange _command, NpcType _type)
	{
		switch (_command)
		{
		case PunishChange.Add:
			punishNpcType.Add(_type);
			gameCanvas.EnableSkipButton();
			break;
		case PunishChange.Remove:
			punishNpcType.Remove(_type);
			if (punishNpcType.Count == 0)
			{
				gameCanvas.EnableSkipButton(false);
			}
			break;
		case PunishChange.Check:
			return punishNpcType.Contains(_type);
		}
		return false;
	}

	public void SkipPunishes()
	{
		while (punishNpcType.Count > 0)
		{
			Sing_Game.This.npcCharacters[(int)(punishNpcType[0] - 2)].PlayerAction();
		}
	}
}
