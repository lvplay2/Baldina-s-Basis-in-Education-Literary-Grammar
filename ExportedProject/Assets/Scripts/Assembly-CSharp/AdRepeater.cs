using UnityEngine;

public class AdRepeater : MonoBehaviour
{
	public float secondsFirst;

	public float secondsRepeating;

	private void Start()
	{
		if (!Shop.This.NoAds)
		{
			InvokeRepeating("_Repeate", secondsFirst, secondsRepeating);
		}
	}

	private void _Repeate()
	{
		AdsController.This.MY_ShowInterstitial();
	}
}
