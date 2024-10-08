using UnityEngine;

public class NPC_TeleportMove : NPC_Main
{
	public Transform teleportPointsParent;

	protected Vector3[] _movePointsPos;

	private int _markerCount;

	protected Vector3 _currentTargetPosition;

	protected override void Awake()
	{
		base.Awake();
		if (teleportPointsParent == null)
		{
			Debug.LogError("Set (teleportPointsParent) variable on " + gobj.name);
			Application.Quit();
		}
		_markerCount = teleportPointsParent.childCount;
		_movePointsPos = new Vector3[_markerCount];
		for (int i = 0; i < _markerCount; i++)
		{
			_movePointsPos[i] = teleportPointsParent.GetChild(i).position;
		}
	}

	public override void PlayerAgro()
	{
		base.PlayerAgro();
	}

	public override void PlayerPunish()
	{
		base.PlayerPunish();
	}

	public override void PlayerAction()
	{
		base.PlayerAction();
	}

	public override void PlayerCollision()
	{
		base.PlayerCollision();
	}

	protected void SetRandomPosition()
	{
		Vector3 vector;
		do
		{
			vector = _movePointsPos[Random.Range(0, _markerCount)];
		}
		while (_currentTargetPosition == vector);
		_currentTargetPosition = vector;
		tform.position = _currentTargetPosition;
	}

	public override void ThedPunish()
	{
		PlayerAction();
	}
}
