using UnityEngine;

public class Door : MonoBehaviour, IObjectArea
{
	public Transform tform;

	public BoxCollider boxCollider;

	private bool isBlock;

	private Vector3 openRot = new Vector3(0f, 90f, 0f);

	private void Awake()
	{
	}

	public void OnAreaEnter()
	{
		if (!isBlock)
		{
			boxCollider.enabled = false;
			Open();
		}
	}

	public void OnAreaEnter_Npc()
	{
		Open();
	}

	public void OnAreaExit()
	{
		boxCollider.enabled = true;
		Close();
	}

	public void OnAreaExit_Npc()
	{
		Close();
	}

	public void SetBlock(bool _isOn)
	{
		isBlock = _isOn;
	}

	private void Open()
	{
		tform.localEulerAngles = openRot;
	}

	private void Close()
	{
		tform.localEulerAngles = Vector3.zero;
	}
}
