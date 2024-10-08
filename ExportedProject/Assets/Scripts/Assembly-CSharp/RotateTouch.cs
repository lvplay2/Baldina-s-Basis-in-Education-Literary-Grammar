using UnityEngine;
using UnityEngine.EventSystems;

public class RotateTouch : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IEventSystemHandler
{
	public bool isDrag;

	public Vector2 rotateAngle;

	private Vector2 _lastPos = Vector2.zero;

	private PointerEventData pointer;

	public void OnPointerDown(PointerEventData eventData)
	{
		pointer = eventData;
		isDrag = true;
		_lastPos = eventData.position;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		isDrag = false;
		rotateAngle = Vector2.zero;
	}

	public Vector2 GetAngle()
	{
		rotateAngle = pointer.position - _lastPos;
		_lastPos = pointer.position;
		return rotateAngle;
	}
}
