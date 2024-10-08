using UnityEngine;

public class UnityAdsSettings : ScriptableObject
{
	public const string defaultIosGameId = "18660";

	public const string defaultAndroidGameId = "18658";

	public string iosGameId;

	public string androidGameId;

	public bool enableTestMode = true;

	public bool showInfoLogs;

	public bool showDebugLogs;

	public bool showWarningLogs = true;

	public bool showErrorLogs = true;

	public UnityAdsSettings()
	{
		iosGameId = "18660";
		androidGameId = "18658";
	}
}
