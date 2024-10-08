using UnityEngine;

public class PlayerController_Game : PlayerController_Main
{
	public Animator cameraAnim;

	public bool keyArea;

	public override void UpdateRotationSensitivity()
	{
		base.UpdateRotationSensitivity();
	}

	public override void OnTriggerEnter(Collider other)
	{
		string text = other.tag;
		if (text != null && text == "ObjectArea")
		{
			other.GetComponent<IObjectArea>().OnAreaEnter();
		}
	}

	public override void OnTriggerStay(Collider other)
	{
	}

	public override void OnTriggerExit(Collider other)
	{
		string text = other.tag;
		if (text != null && text == "ObjectArea")
		{
			other.GetComponent<IObjectArea>().OnAreaExit();
		}
	}

	public override void LeftClickAction()
	{
		Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, takeDistance) && hitInfo.transform.tag == "Boost")
		{
			hitInfo.transform.GetComponent<IObjectClick>().OnClick();
		}
	}

	public override void RightClickAction()
	{
		Sing_Game.This.gameCanvas.UseBoost();
	}

	public void QueenRotation(Vector3 _pos)
	{
		Vector3 forward = _pos - base.transform.position;
		forward.y = 0f;
		base.transform.rotation = Quaternion.LookRotation(forward);
	}

	public void CameraFall(bool isOn)
	{
		cameraAnim.SetBool("Fall", isOn);
	}
}
