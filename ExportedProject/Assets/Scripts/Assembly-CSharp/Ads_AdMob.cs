using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ads_AdMob : MonoBehaviour
{
	private bool _isUseBanner;

	private string _bannerId_ANDROID;

	private string _bannerId_IPHONE;

	private AdPosition _positionBanner;

	private BannerView _banner;

	private bool _isSettedHandlersBanner;

	private List<bool> _isShowBannerList;

	private string _interstitialId_ANDROID;

	private string _interstitialId_IPHONE;

	private InterstitialAd _interstitial;

	private bool _isSettedHandlersInterstitial;

	private string _rewardedVideoId_ANDROID;

	private string _rewardedVideoId_IPHONE;

	private RewardBasedVideoAd _rewardedVideo;

	private bool _isSettedHandlersRewardedVideo;

	private Action _actionRewarded;

	public void MY_Init(bool isUseBanner, string bannerId_ANDROID, string bannerId_IPHONE, AdPosition positionBanner, List<bool> isShowBannerList, string interstitialId_ANDROID, string interstitialId_IPHONE, string rewardedVideoId_ANDROID, string rewardedVideoId_IPHONE)
	{
		_Init(isUseBanner, bannerId_ANDROID, bannerId_IPHONE, positionBanner, isShowBannerList, interstitialId_ANDROID, interstitialId_IPHONE, rewardedVideoId_ANDROID, rewardedVideoId_IPHONE);
	}

	public void MY_ShowBanner()
	{
		_banner.Show();
	}

	public void MY_HideBanner()
	{
		_banner.Hide();
	}

	public void MY_ShowInterstitial()
	{
		if (_interstitial != null && _interstitial.IsLoaded())
		{
			_interstitial.Show();
		}
	}

	public void MY_ShowRewardedVideo(Action actionRewarded)
	{
		if (_rewardedVideo != null && _rewardedVideo.IsLoaded())
		{
			_actionRewarded = actionRewarded;
			_rewardedVideo.Show();
		}
	}

	public bool MY_IsInterstitialReady()
	{
		return _interstitial != null && _interstitial.IsLoaded();
	}

	public bool MY_IsRewardedVideoReady()
	{
		return _rewardedVideo != null && _rewardedVideo.IsLoaded();
	}

	private void _Init(bool isUseBanner, string bannerId_ANDROID, string bannerId_IPHONE, AdPosition positionBanner, List<bool> isShowBannerList, string interstitialId_ANDROID, string interstitialId_IPHONE, string rewardedVideoId_ANDROID, string rewardedVideoId_IPHONE)
	{
		SceneManager.sceneLoaded += _SceneLoaded;
		_isUseBanner = isUseBanner;
		_bannerId_ANDROID = bannerId_ANDROID;
		_bannerId_IPHONE = bannerId_IPHONE;
		_positionBanner = positionBanner;
		_isShowBannerList = isShowBannerList;
		_interstitialId_ANDROID = interstitialId_ANDROID;
		_interstitialId_IPHONE = interstitialId_IPHONE;
		_rewardedVideoId_ANDROID = rewardedVideoId_ANDROID;
		_rewardedVideoId_IPHONE = rewardedVideoId_IPHONE;
		_InitBanner();
		_InitInterstitial();
		_InitRewardedVideo();
	}

	private void _SceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		if (_isUseBanner)
		{
			_ShowBanner();
		}
	}

	private void _InitBanner()
	{
		if (_isUseBanner)
		{
			if (_banner != null)
			{
				_banner.Destroy();
			}
			string bannerId_IPHONE = _bannerId_IPHONE;
			_banner = new BannerView(bannerId_IPHONE, AdSize.Banner, _positionBanner);
			AdRequest request = new AdRequest.Builder().Build();
			_banner.LoadAd(request);
			if (!_isSettedHandlersBanner)
			{
				_isSettedHandlersBanner = true;
				_banner.OnAdLoaded += _BannerLoaded;
				_banner.OnAdOpening += _BannerOpening;
				_banner.OnAdFailedToLoad += _BanneFailedToLoad;
				_banner.OnAdClosed += _BannerClosed;
			}
		}
	}

	private void _BannerLoaded(object sender, EventArgs args)
	{
		if (!_isUseBanner)
		{
			return;
		}
		int buildIndex = SceneManager.GetActiveScene().buildIndex;
		if (buildIndex < _isShowBannerList.Count)
		{
			if (_isShowBannerList[buildIndex])
			{
				MY_ShowBanner();
			}
			else
			{
				MY_HideBanner();
			}
		}
		else
		{
			MY_ShowBanner();
		}
	}

	private void _BannerOpening(object sender, EventArgs args)
	{
	}

	private void _BanneFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		if (_isUseBanner)
		{
			_InitBanner();
		}
	}

	private void _BannerClosed(object sender, EventArgs args)
	{
		if (_isUseBanner)
		{
			_InitBanner();
		}
	}

	private void _ShowBanner()
	{
		Scene activeScene = SceneManager.GetActiveScene();
		if (activeScene.buildIndex < _isShowBannerList.Count)
		{
			if (_isShowBannerList[activeScene.buildIndex])
			{
				MY_ShowBanner();
			}
			else
			{
				MY_HideBanner();
			}
		}
		else
		{
			MY_ShowBanner();
		}
	}

	private void _InitInterstitial()
	{
		string interstitialId_IPHONE = _interstitialId_IPHONE;
		_interstitial = new InterstitialAd(interstitialId_IPHONE);
		if (!_isSettedHandlersInterstitial)
		{
			_isSettedHandlersInterstitial = true;
			_interstitial.OnAdLoaded += _InterstitialLoaded;
			_interstitial.OnAdOpening += _InterstitialOpening;
			_interstitial.OnAdFailedToLoad += _InterstitialFailedToLoad;
			_interstitial.OnAdClosed += _InterstitialClosed;
		}
		AdRequest request = new AdRequest.Builder().Build();
		_interstitial.LoadAd(request);
	}

	private void _InterstitialLoaded(object sender, EventArgs args)
	{
	}

	private void _InterstitialOpening(object sender, EventArgs args)
	{
	}

	private void _InterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		_InitInterstitial();
	}

	private void _InterstitialClosed(object sender, EventArgs args)
	{
		_InitInterstitial();
	}

	private void _InitRewardedVideo()
	{
		string rewardedVideoId_IPHONE = _rewardedVideoId_IPHONE;
		_rewardedVideo = RewardBasedVideoAd.Instance;
		if (!_isSettedHandlersRewardedVideo)
		{
			_rewardedVideo.OnAdLoaded += _RewardVideoLoaded;
			_rewardedVideo.OnAdFailedToLoad += _RewardVideoFailedToLoad;
			_rewardedVideo.OnAdOpening += _RewardVideoOpening;
			_rewardedVideo.OnAdStarted += _RewardVideoStarted;
			_rewardedVideo.OnAdRewarded += _RewardVideoRewarded;
			_rewardedVideo.OnAdClosed += _RewardVideoClosed;
			_rewardedVideo.OnAdLeavingApplication += _RewardVideoLeavingApplication;
			_isSettedHandlersRewardedVideo = true;
		}
		AdRequest request = new AdRequest.Builder().Build();
		_rewardedVideo.LoadAd(request, rewardedVideoId_IPHONE);
	}

	private void _RewardVideoLoaded(object sender, EventArgs args)
	{
	}

	private void _RewardVideoOpening(object sender, EventArgs args)
	{
	}

	private void _RewardVideoStarted(object sender, EventArgs args)
	{
	}

	private void _RewardVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		_InitRewardedVideo();
	}

	private void _RewardVideoClosed(object sender, EventArgs args)
	{
		_InitRewardedVideo();
	}

	private void _RewardVideoLeavingApplication(object sender, EventArgs args)
	{
	}

	private void _RewardVideoRewarded(object sender, Reward args)
	{
		if (_actionRewarded != null)
		{
			_actionRewarded();
		}
	}
}
