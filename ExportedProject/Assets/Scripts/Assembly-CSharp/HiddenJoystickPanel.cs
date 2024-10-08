using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HiddenJoystickPanel : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler, IEventSystemHandler
{
	public bool isDrag;

	public GameObject joysticObj;

	public RectTransform screenParent;

	public RectTransform backStickTfrom;

	public RectTransform frontStickTfrom;

	public Image backStick;

	public Image frontStick;

	private Vector2 direction;

	private Color hideColor = new Color(1f, 1f, 1f, 0f);

	private Vector2 backPos;

	private Vector2 screenMax;

	private Vector2 screenMaxBack;

	private void Awake()
	{
		screenMax = new Vector2((float)Screen.width / 2.4f, (float)Screen.height / 1.8f);
		screenMaxBack = new Vector2(screenMax.x - 50f, screenMax.y - 50f);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		joysticObj.SetActive(true);
		StopCoroutine("HideJoystick");
		hideColor.a = 1f;
		backStick.color = hideColor;
		frontStick.color = hideColor;
		backPos = eventData.position;
		backStickTfrom.position = backPos;
		frontStickTfrom.localPosition = Vector2.zero;
		direction = Vector2.zero;
		isDrag = true;
	}

	public void OnDrag(PointerEventData eventData)
	{
		Vector2 position = eventData.position;
		if (position.x > screenMax.x)
		{
			position.x = screenMax.x;
		}
		if (position.y > screenMax.y)
		{
			position.y = screenMax.y;
		}
		float num = Vector2.Distance(backPos, position);
		if (num < 50f)
		{
			frontStickTfrom.position = position;
		}
		else
		{
			Vector2 vector = (position - backPos).normalized * (num - 50f);
			backPos += vector;
			if (backPos.x > screenMaxBack.x)
			{
				backPos.x = screenMaxBack.x;
			}
			if (backPos.y > screenMaxBack.y)
			{
				backPos.y = screenMaxBack.y;
			}
			backStickTfrom.position = backPos;
			frontStickTfrom.position = position;
		}
		direction = (position - backPos).normalized;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		isDrag = false;
		StartCoroutine("HideJoystick");
	}

	private IEnumerator HideJoystick()
	{
		while (hideColor.a > 0f)
		{
			hideColor.a -= 2f * Time.deltaTime;
			backStick.color = hideColor;
			frontStick.color = hideColor;
			yield return null;
		}
		hideColor.a = 0f;
		backStick.color = hideColor;
		frontStick.color = hideColor;
		joysticObj.SetActive(false);
	}

	public Vector2 GetDirection()
	{
		return direction;
	}
}
