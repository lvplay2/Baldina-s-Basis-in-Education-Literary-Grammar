using System;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Menu : Canvas_Main
{
	[Serializable]
	public struct MenuCanvasElements
	{
		public string name;

		public Slider sliderSensitivity;

		public Outline outlineSensitivity;

		public GameObject buttonAudioOn;

		public GameObject buttonAudioOff;
	}

	public MenuCanvasElements[] menuCanvasElements;

	public Text text_SkipPunishmentsCount;

	public GameObject go_PanelLoading;

	public GameObject go_PanelQuit;

	public GameObject shopButton;

	public GameObject panelPc;

	public GameObject panelMobile;

	public Text skipCountText;

	private Slider slider_Sensitivity;

	private Outline outline_Sensitivity;

	private GameObject go_ButtonAudioOn;

	private GameObject go_ButtonAudioOff;

	private void Start()
	{
		if (GameplayManager.This.currentDevice == GameplayManager.DeviceType.Mobile)
		{
			panelMobile.SetActive(true);
			InitVariables(1);
		}
		else
		{
			panelPc.SetActive(true);
			InitVariables(0);
		}
		go_ButtonAudioOn.SetActive(!GameplayManager.This.isAudio);
		go_ButtonAudioOff.SetActive(GameplayManager.This.isAudio);
		slider_Sensitivity.value = GameplayManager.This.sensitivity;
		float num = (slider_Sensitivity.value - 0.5f) / 1.5f;
		float r = 1f - num;
		outline_Sensitivity.effectColor = new Color(r, num, 0f);
		if (BaldinaShop.This.isUnlimited)
		{
			shopButton.SetActive(false);
		}
		skipCountText.text = BaldinaShop.This.currentSkipCount.ToString();
	}

	public void UpdateVariables()
	{
		skipCountText.text = BaldinaShop.This.currentSkipCount.ToString();
	}

	private void InitVariables(int _platform)
	{
		slider_Sensitivity = menuCanvasElements[_platform].sliderSensitivity;
		outline_Sensitivity = menuCanvasElements[_platform].outlineSensitivity;
		go_ButtonAudioOn = menuCanvasElements[_platform].buttonAudioOn;
		go_ButtonAudioOff = menuCanvasElements[_platform].buttonAudioOff;
	}

	public void UA_Start()
	{
		BaldinaShop.This.UA_OpenBoostsGame();
	}

	public void EnableLoadingCanvas(bool _isOn = true)
	{
		go_PanelLoading.SetActive(_isOn);
	}

	public void UA_JoinUsToFacebook()
	{
		Application.OpenURL(SocialManager.This.url_JoinUsToFacebook);
	}

	public void UA_SetSensitivity(float _value)
	{
		GameplayManager.This.sensitivity = _value;
		float num = (_value - 0.5f) / 1.5f;
		float r = 1f - num;
		outline_Sensitivity.effectColor = new Color(r, num, 0f);
	}

	public void UA_AudioOn(bool _isOn)
	{
		GameplayManager.This.isAudio = _isOn;
		AudioListener.volume = (_isOn ? 1 : 0);
		go_ButtonAudioOn.SetActive(!_isOn);
		go_ButtonAudioOff.SetActive(_isOn);
	}

	public void _Quit()
	{
		Application.Quit();
	}

	public void UA_Quit(bool _isOn)
	{
		go_PanelQuit.SetActive(_isOn);
	}

	public void UA_RateUs()
	{
		switch (GameplayManager.This.currentPlayform)
		{
		case GameplayManager.PlatformType.Android:
			Application.OpenURL(SocialManager.This.url_RateUsGooglePlay);
			break;
		case GameplayManager.PlatformType.iOS:
			Application.OpenURL(SocialManager.This.url_RateUsAppStore);
			break;
		}
	}

	public void UA_MoreGames()
	{
		switch (GameplayManager.This.currentPlayform)
		{
		case GameplayManager.PlatformType.Android:
			Application.OpenURL(SocialManager.This.url_MoreGamesUsGooglePlay);
			break;
		case GameplayManager.PlatformType.iOS:
			Application.OpenURL(SocialManager.This.url_MoreGamesUsAppStore);
			break;
		}
	}

	public void UA_OpenShop()
	{
		BaldinaShop.This.UA_Open();
	}

	public void UA_OpenShopDisables()
	{
		BaldinaShop.This.UA_Open();
		BaldinaShop.This.UA_ChangeButton(3);
	}

	public void UA_DisableAds()
	{
	}

	public void UA_RestorePurchases()
	{
	}
}
