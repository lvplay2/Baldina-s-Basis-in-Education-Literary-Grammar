using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenItemClick : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	public PlayerController_Game playerController;

	public void OnPointerDown(PointerEventData eventData)
	{
		playerController.LeftClickAction();
	}
}
