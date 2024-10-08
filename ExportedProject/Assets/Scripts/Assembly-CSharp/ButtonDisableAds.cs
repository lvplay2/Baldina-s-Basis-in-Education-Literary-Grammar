using UnityEngine;
using UnityEngine.UI;

public class ButtonDisableAds : MonoBehaviour
{
	private Button _button;

	private void Start()
	{
		_button = GetComponent<Button>();
		_button.onClick.AddListener(OnClick);
	}

	private void Update()
	{
		_button.interactable = !Shop.This.NoAds;
	}

	public void OnClick()
	{
		Shop.This.UA_DisableAds();
	}
}
