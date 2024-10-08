using System.Collections;
using UnityEngine;

public class ShowAdOnLoad : MonoBehaviour
{
	public string zoneId;

	public bool enableTimeouts = true;

	public float initTimeout = 15f;

	public float showTimeout = 15f;

	public float yieldTime = 0.5f;

	private float _startTime;

	private IEnumerator Start()
	{
		if (!UnityAdsHelper.isSupported)
		{
			yield break;
		}
		string zoneName = ((!string.IsNullOrEmpty(zoneId)) ? zoneId : "the default ad placement zone");
		_startTime = Time.timeSinceLevelLoad;
		while (!UnityAdsHelper.isInitialized)
		{
			if (enableTimeouts && Time.timeSinceLevelLoad - _startTime > initTimeout)
			{
				Debug.LogWarning(string.Format("Unity Ads failed to initialize in a timely manner. An ad for {0} will not be shown on load.", zoneName));
				yield break;
			}
			yield return new WaitForSeconds(yieldTime);
		}
		Debug.Log("Unity Ads has finished initializing. Waiting for ads to be ready...");
		_startTime = Time.timeSinceLevelLoad;
		while (!UnityAdsHelper.IsReady(zoneId))
		{
			if (enableTimeouts && Time.timeSinceLevelLoad - _startTime > showTimeout)
			{
				Debug.LogWarning(string.Format("Unity Ads failed to be ready in a timely manner. An ad for {0} will not be shown on load.", zoneName));
				yield break;
			}
			yield return new WaitForSeconds(yieldTime);
		}
		Debug.Log(string.Format("Ads for {0} are available and ready. Showing ad now...", zoneName));
		UnityAdsHelper.ShowAd(zoneId);
	}
}
