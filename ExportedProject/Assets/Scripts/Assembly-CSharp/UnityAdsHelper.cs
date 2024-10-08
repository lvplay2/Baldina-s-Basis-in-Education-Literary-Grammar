using System;
using UnityEngine;

public class UnityAdsHelper : MonoBehaviour
{
	public static Action onFinished;

	public static Action onSkipped;

	public static Action onFailed;

	private static string _gamerSID;

	public static string gamerSID
	{
		get
		{
			return _gamerSID;
		}
	}

	public static bool isShowing
	{
		get
		{
			return false;
		}
	}

	public static bool isSupported
	{
		get
		{
			return false;
		}
	}

	public static bool isInitialized
	{
		get
		{
			return false;
		}
	}

	public static void SetGamerSID(string gamerSID)
	{
		gamerSID = gamerSID.Trim();
		_gamerSID = ((!string.IsNullOrEmpty(gamerSID)) ? gamerSID : null);
	}

	public static void Initialize()
	{
		Debug.LogError("Failed to initialize Unity Ads. Current build platform is not supported.");
	}

	public static bool IsReady()
	{
		return false;
	}

	public static bool IsReady(string zoneId)
	{
		return false;
	}

	public static void ShowAd()
	{
		ShowAd(null);
	}

	public static void ShowAd(string zoneId)
	{
		Debug.LogError("Failed to show ad. Unity Ads does not support the current build platform.");
	}
}
