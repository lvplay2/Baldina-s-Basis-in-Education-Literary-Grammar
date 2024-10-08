using UnityEngine;

public class CameraFallEvent : MonoBehaviour
{
	public PlayerController_Game playerController;

	public void CameraStandEnd()
	{
		playerController.EnableControlls(true);
	}
}
