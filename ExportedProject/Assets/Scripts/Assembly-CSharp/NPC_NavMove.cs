using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPC_NavMove : NPC_Main
{
	public NavMeshAgent navMeshAgent;

	public Transform navMovePointsParent;

	public Transform raycastStart;

	protected Vector3[] _movePointsPos;

	private int _markerCount;

	protected Vector3 _currentTargetPosition;

	private IEnumerator moveToTargetCoroutine;

	protected override void Awake()
	{
		base.Awake();
		if (navMeshAgent == null)
		{
			navMeshAgent = GetComponent<NavMeshAgent>();
			Debug.LogWarning("Set (navMeshAgent) variable on " + gobj.name);
		}
		if (navMovePointsParent == null)
		{
			Debug.LogError("Set (navMovePointsParent) variable on " + gobj.name);
			Application.Quit();
		}
		_markerCount = navMovePointsParent.childCount;
		_movePointsPos = new Vector3[_markerCount];
		for (int i = 0; i < _markerCount; i++)
		{
			_movePointsPos[i] = navMovePointsParent.GetChild(i).position;
		}
	}

	public virtual void ActivateNav(bool isOn = true)
	{
		navMeshAgent.enabled = isOn;
		if (!isOn)
		{
			EnableFollow(false);
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

	public virtual void EnterMarker(Collider other)
	{
		if (IsState(NpcState.MarkerMove))
		{
			NextMarker(other.transform.position);
		}
	}

	protected void SetTarget(Vector3 _position)
	{
		_currentTargetPosition = _position;
		if (navMeshAgent.enabled)
		{
			navMeshAgent.SetDestination(_position);
		}
	}

	protected void NextMarker(Vector3 _position)
	{
		if (_currentTargetPosition == _position)
		{
			SetRandomTarget();
		}
	}

	protected void SetRandomTarget()
	{
		Vector3 vector;
		do
		{
			vector = _movePointsPos[Random.Range(0, _markerCount)];
		}
		while (_currentTargetPosition == vector);
		SetTarget(vector);
	}

	protected void EnableFollow(bool _isOn, Transform _target = null)
	{
		if (_isOn)
		{
			if (moveToTargetCoroutine != null)
			{
				StopCoroutine(moveToTargetCoroutine);
			}
			moveToTargetCoroutine = MoveToTarget(_target);
			StartCoroutine(moveToTargetCoroutine);
		}
		else if (moveToTargetCoroutine != null)
		{
			StopCoroutine(moveToTargetCoroutine);
		}
	}

	private IEnumerator MoveToTarget(Transform _target = null)
	{
		while (true)
		{
			SetTarget(_target.position);
			yield return new WaitForSeconds(0.2f);
		}
	}

	public override void EnterZone()
	{
		base.EnterZone();
	}

	public override void StayZone()
	{
		base.StayZone();
	}

	public override void ExitZone()
	{
		base.ExitZone();
	}

	public override void BoostAction(BoostType _type)
	{
		base.BoostAction(_type);
	}

	public override void ThedPunish()
	{
		Sing_Game.This.birds.gameObject.SetActive(true);
		Sing_Game.This.birds.parent = tform;
		Sing_Game.This.birds.localPosition = new Vector3(0f, 1.8f, 0f);
		ActivateNav(false);
		Invoke("ThedPunishEnd", GameplayManager.This.thed_npcPunishTime);
	}

	private void ThedPunishEnd()
	{
		Sing_Game.This.birds.gameObject.SetActive(false);
		ActivateNav();
		SetNewState(npcState);
	}

	public override void DisableInGame()
	{
		base.DisableInGame();
		navMovePointsParent.gameObject.SetActive(false);
	}
}
