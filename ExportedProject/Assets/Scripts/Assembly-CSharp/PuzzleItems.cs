using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleItems : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IEventSystemHandler
{
	public RectTransform rectTform;

	private Transform par;

	public Vector3 startPos;

	public int placeId;

	public bool isCorrect;

	public bool inPlace;

	public Text itemText;

	private void Start()
	{
		inPlace = false;
		isCorrect = false;
		itemText.text = Story_Game.This.puzzleTexts[Story_Game.This.randomPuzzleText][placeId];
		par = base.transform.parent;
		startPos = rectTform.transform.position;
	}

	public void InitPlace(float _y)
	{
		Vector3 localPosition = rectTform.transform.localPosition;
		localPosition.y = _y;
		rectTform.transform.localPosition = localPosition;
		startPos = rectTform.transform.position;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		rectTform.SetParent(null);
		rectTform.SetParent(par);
	}

	public void OnDrag(PointerEventData eventData)
	{
		rectTform.transform.position = eventData.position;
	}

	public void OnDrop(PointerEventData eventData)
	{
		isCorrect = false;
		inPlace = false;
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, list);
		foreach (RaycastResult item in list)
		{
			if (item.gameObject.name.Contains("Slot") && item.gameObject.name != base.name)
			{
				break;
			}
			if (item.gameObject.name.Contains("Place"))
			{
				rectTform.transform.position = item.gameObject.transform.position;
				int result;
				int.TryParse(item.gameObject.name[5].ToString(), out result);
				inPlace = true;
				if (result == placeId)
				{
					isCorrect = true;
				}
				return;
			}
		}
		rectTform.transform.position = startPos;
	}
}
