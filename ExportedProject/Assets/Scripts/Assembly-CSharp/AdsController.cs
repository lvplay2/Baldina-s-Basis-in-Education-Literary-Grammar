using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdsController : MonoBehaviour
{
	[Header("UNITY")]
	public Ads_Unity adsUnity;

	[Header("Unity Ads Data")]
	public string gameID_ANDROID = string.Empty;

	public string gameID_IPHONE = string.Empty;

	[Header("ADMOB")]
	public Ads_AdMob adsAdMob;

	[Header("Banner")]
	public bool isUseBanner = true;

	public string bannerId_ANDROID;

	public string bannerId_IPHONE;

	public AdPosition positionBanner;

	[Header("Interstitial")]
	public string interstitialId_ANDROID;

	public string interstritialId_IPHONE;

	[Header("Rewarded video")]
	public string rewardedVideoId_ANDROID;

	public string rewardedVideoId_IPHONE;

	[Header("UI Components")]
	public GameObject go_PanelPause;

	public GameObject go_PanelAdsNotLoaded;

	[HideInInspector]
	public List<string> scenesList;

	[HideInInspector]
	public List<bool> isShowInterstitialList;

	[HideInInspector]
	public List<bool> isShowBannerList;

	[HideInInspector]
	public bool isShowAdsWhenSceneLoaded = true;

	private string _defaultZone = "video";

	private string _rewardedZone = "rewardedVideo";

	private bool _isRewardedUnityTurn;

	private bool _isInterstitialUnityTurn;

	private bool _isFirstLaunch;

	private string _firstSceneName;

	private float _gameTimeScale = 1f;

	public static AdsController This { get; private set; }

	public bool IsRewardedUnityTurn
	{
		get
		{
			return _isRewardedUnityTurn;
		}
		set
		{
			_isRewardedUnityTurn = value;
			PlayerPrefs.SetInt("AdsController: IsRewardedUnityTurn", value ? 1 : 0);
			PlayerPrefs.Save();
		}
	}

	public bool IsInterstitialUnityTurn
	{
		get
		{
			return _isInterstitialUnityTurn;
		}
		set
		{
			_isInterstitialUnityTurn = value;
			PlayerPrefs.SetInt("AdsController: IsInterstitialUnityTurn", value ? 1 : 0);
			PlayerPrefs.Save();
		}
	}

	public bool IsFirstLaunch
	{
		get
		{
			return _isFirstLaunch;
		}
		set
		{
			_isFirstLaunch = value;
			PlayerPrefs.SetInt("AdsController: IFirstLaunch", value ? 1 : 0);
			PlayerPrefs.Save();
		}
	}

	private void Awake()
	{
		if (This != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		This = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		_Init();
	}

	public void MY_ShowRewardedVideo(Action actionRewarded)
	{
		MY_GamePause(true);
		if (IsRewardedUnityTurn)
		{
			if (adsUnity.MY_IsRewardedVideoReady())
			{
				adsUnity.MY_RewardedVideoShow(actionRewarded);
			}
			else
			{
				adsAdMob.MY_ShowRewardedVideo(actionRewarded);
			}
		}
		else if (adsAdMob.MY_IsRewardedVideoReady())
		{
			adsAdMob.MY_ShowRewardedVideo(actionRewarded);
		}
		else
		{
			adsUnity.MY_RewardedVideoShow(actionRewarded);
		}
		IsRewardedUnityTurn = !IsRewardedUnityTurn;
	}

	public void MY_ShowInterstitial()
	{
		MY_GamePause(true);
		if (IsInterstitialUnityTurn)
		{
			if (adsUnity.MY_IsVideoReady())
			{
				adsUnity.MY_VideoShow();
			}
			else if (adsAdMob.MY_IsInterstitialReady())
			{
				adsAdMob.MY_ShowInterstitial();
			}
			else
			{
				MY_VideoIsNotLoaded();
			}
		}
		else if (adsAdMob.MY_IsInterstitialReady())
		{
			adsAdMob.MY_ShowInterstitial();
		}
		else if (adsUnity.MY_IsVideoReady())
		{
			adsUnity.MY_VideoShow();
		}
		else
		{
			MY_VideoIsNotLoaded();
		}
		IsInterstitialUnityTurn = !IsInterstitialUnityTurn;
	}

	public bool MY_IsRewardedReady()
	{
		return adsUnity.MY_IsRewardedVideoReady() || adsAdMob.MY_IsRewardedVideoReady();
	}

	public void MY_GamePause(bool isPause)
	{
		_gameTimeScale = ((!isPause) ? _gameTimeScale : Time.timeScale);
		Time.timeScale = ((!isPause) ? _gameTimeScale : 0f);
		AudioListener.pause = isPause;
		go_PanelPause.SetActive(isPause);
	}

	public void MY_VideoIsNotLoaded()
	{
		MY_GamePause(true);
		go_PanelAdsNotLoaded.SetActive(true);
	}

	public void UA_ClosePanelAdsNotLoaded()
	{
		MY_GamePause(false);
		go_PanelAdsNotLoaded.SetActive(false);
	}

	public void UA_Continue()
	{
		MY_GamePause(false);
	}

	private void _Init()
	{
		if (!PlayerPrefs.HasKey("AdsController: IsRewardedUnityTurn"))
		{
			PlayerPrefs.SetInt("AdsController: IsRewardedUnityTurn", 1);
		}
		if (!PlayerPrefs.HasKey("AdsController: IsInterstitialUnityTurn"))
		{
			PlayerPrefs.SetInt("AdsController: IsInterstitialUnityTurn", 1);
		}
		if (!PlayerPrefs.HasKey("AdsController: IFirstLaunch"))
		{
			PlayerPrefs.SetInt("AdsController: IFirstLaunch", 1);
		}
		PlayerPrefs.Save();
		SceneManager.sceneLoaded += _SceneLoaded;
		isUseBanner = ((!isUseBanner) ? isUseBanner : (!Shop.This.NoAds));
		isShowAdsWhenSceneLoaded = ((!isShowAdsWhenSceneLoaded) ? isShowAdsWhenSceneLoaded : (!Shop.This.NoAds));
		_isRewardedUnityTurn = PlayerPrefs.GetInt("AdsController: IsRewardedUnityTurn", 1) == 1;
		_isInterstitialUnityTurn = PlayerPrefs.GetInt("AdsController: IsInterstitialUnityTurn", 1) == 1;
		_isFirstLaunch = PlayerPrefs.GetInt("AdsController: IFirstLaunch", 1) == 1;
		adsUnity.MY_Init(gameID_ANDROID, gameID_IPHONE, _defaultZone, _rewardedZone);
		adsAdMob.MY_Init(isUseBanner, bannerId_ANDROID, bannerId_IPHONE, positionBanner, isShowBannerList, interstitialId_ANDROID, interstritialId_IPHONE, rewardedVideoId_ANDROID, rewardedVideoId_IPHONE);
	}

	private void _SceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		if (IsFirstLaunch)
		{
			_firstSceneName = arg0.name;
			IsFirstLaunch = false;
			return;
		}
		if (_firstSceneName != null)
		{
			if (_firstSceneName != arg0.name)
			{
				return;
			}
			_firstSceneName = null;
		}
		if (isShowAdsWhenSceneLoaded && (arg0.buildIndex >= isShowInterstitialList.Count || isShowInterstitialList[arg0.buildIndex]))
		{
			MY_ShowInterstitial();
		}
	}
}
