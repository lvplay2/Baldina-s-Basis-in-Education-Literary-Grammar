using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour, IObjectClick
{
	public BoostType boostType;

	public GameObject gobj;

	public Transform tform;

	public Transform startParent;

	public bool isRestoring;

	public float restoringTime;

	public Transform spawnMarkerParent;

	public bool isActive;

	private List<Transform> _spawnMarkers = new List<Transform>();

	private int _spawnMarkersCount;

	private void Awake()
	{
		startParent = tform.parent;
		foreach (Transform item in spawnMarkerParent)
		{
			_spawnMarkers.Add(item);
		}
		_spawnMarkersCount = _spawnMarkers.Count;
		if (_spawnMarkersCount == 0)
		{
			Debug.LogWarning("No marker points in " + gobj.name);
		}
		if (boostType != BoostType.GoldBall)
		{
			InitBoost();
		}
	}

	public void UpdateParent()
	{
		tform.parent = startParent;
	}

	public void InitBoost()
	{
		isActive = false;
		if (_spawnMarkersCount == 1)
		{
			tform.position = _spawnMarkers[0].position;
		}
		else
		{
			Vector3 position;
			do
			{
				position = _spawnMarkers[Random.Range(0, _spawnMarkersCount)].position;
			}
			while (tform.position == position);
			tform.position = position;
		}
		gobj.SetActive(true);
	}

	public void OnClick()
	{
		Sing_Game.This.gameCanvas.AddBoost(boostType);
		gobj.SetActive(false);
	}

	private void RestoreBoost()
	{
		InitBoost();
	}

	public void PlaceBoost(Vector3 _pos)
	{
		_pos.y = 0f;
		isActive = true;
		gobj.SetActive(true);
		tform.localPosition = _pos;
	}

	public bool NpcAction()
	{
		if (isActive)
		{
			gobj.SetActive(false);
		}
		return isActive;
	}
}
