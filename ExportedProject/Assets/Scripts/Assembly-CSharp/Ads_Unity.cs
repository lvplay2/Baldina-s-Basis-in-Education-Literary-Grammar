using System;
using UnityEngine;

public class Ads_Unity : MonoBehaviour
{
	private string _gameID_ANDROID = string.Empty;

	private string _gameID_IPHONE = string.Empty;

	private string _defaultZone = "video";

	private string _rewardedZone = "rewardedVideo";

	public void MY_Init(string gameID_ANDROID, string gameID_IPHONE, string defaultZone, string rewardedZone)
	{
		_Init(gameID_ANDROID, gameID_IPHONE, defaultZone, rewardedZone);
	}

	public void MY_RewardedVideoShow(Action rewardAction, Action onSkippedAction = null)
	{
		if (UnityAdsHelper.IsReady(_rewardedZone))
		{
			UnityAdsHelper.onFinished = rewardAction;
			UnityAdsHelper.onSkipped = onSkippedAction;
			UnityAdsHelper.ShowAd(_rewardedZone);
		}
	}

	public void MY_VideoShow(Action onFinishedAction = null, Action onSkippedAction = null)
	{
		if (UnityAdsHelper.IsReady(_defaultZone))
		{
			UnityAdsHelper.onFinished = onFinishedAction;
			UnityAdsHelper.onSkipped = onSkippedAction;
			UnityAdsHelper.ShowAd(_defaultZone);
		}
	}

	public bool MY_IsVideoReady()
	{
		return UnityAdsHelper.IsReady(_defaultZone);
	}

	public bool MY_IsRewardedVideoReady()
	{
		return UnityAdsHelper.IsReady(_rewardedZone);
	}

	private void _Init(string gameID_ANDROID, string gameID_IPHONE, string defaultZone, string rewardedZone)
	{
		_gameID_ANDROID = gameID_ANDROID;
		_gameID_IPHONE = gameID_IPHONE;
		_defaultZone = defaultZone;
		_rewardedZone = rewardedZone;
		UnityAdsSettings unityAdsSettings = (UnityAdsSettings)Resources.Load("UnityAdsSettings");
		if (!(unityAdsSettings == null))
		{
			unityAdsSettings.androidGameId = _gameID_ANDROID;
			unityAdsSettings.iosGameId = _gameID_IPHONE;
			UnityAdsHelper.Initialize();
		}
	}
}
