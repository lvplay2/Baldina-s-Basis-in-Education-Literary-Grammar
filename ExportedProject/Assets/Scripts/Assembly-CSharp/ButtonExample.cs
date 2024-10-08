using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonExample : MonoBehaviour
{
	public Text textReady;

	public Text textWaiting;

	public string zoneId;

	public float cooldownTime = 300f;

	public int rewardAmount = 250;

	private string _keyCooldownTime = "CooldownTime";

	private DateTime _rewardCooldownTime;

	private Button _button;

	private void Awake()
	{
		_button = GetComponent<Button>();
		_keyCooldownTime = _keyCooldownTime + base.name + base.gameObject.GetInstanceID();
		_rewardCooldownTime = GetCooldownTime();
	}

	private void Update()
	{
		if ((bool)_button)
		{
			_button.interactable = IsReady();
			if ((bool)textReady)
			{
				textReady.enabled = _button.interactable;
			}
			if ((bool)textWaiting)
			{
				textWaiting.enabled = !_button.interactable;
			}
		}
	}

	private bool IsReady()
	{
		if (DateTime.Compare(DateTime.UtcNow, _rewardCooldownTime) > 0)
		{
			return UnityAdsHelper.IsReady(zoneId);
		}
		return false;
	}

	public void ShowAd()
	{
		UnityAdsHelper.onFinished = OnFinished;
		UnityAdsHelper.ShowAd(zoneId);
	}

	private void OnFinished()
	{
		if (rewardAmount > 0)
		{
			Debug.Log("The player has earned a reward!");
		}
		if (cooldownTime > 0f)
		{
			SetCooldownTime(DateTime.UtcNow.AddSeconds(cooldownTime));
			Debug.Log(string.Format("Next ad is available in {0} seconds.", cooldownTime));
		}
	}

	private DateTime GetCooldownTime()
	{
		if (object.Equals(_rewardCooldownTime, default(DateTime)))
		{
			if (PlayerPrefs.HasKey(_keyCooldownTime))
			{
				_rewardCooldownTime = DateTime.Parse(PlayerPrefs.GetString(_keyCooldownTime));
				if (Debug.isDebugBuild)
				{
					DateTime t = DateTime.UtcNow.AddSeconds(-1f * Time.time);
					DateTime t2 = _rewardCooldownTime.AddSeconds(-1f * cooldownTime);
					if (DateTime.Compare(t, t2) > 0)
					{
						ResetCooldownTime();
					}
				}
			}
			else
			{
				_rewardCooldownTime = DateTime.UtcNow;
			}
		}
		return _rewardCooldownTime;
	}

	private void SetCooldownTime(DateTime dateTime)
	{
		_rewardCooldownTime = dateTime;
		PlayerPrefs.SetString(_keyCooldownTime, dateTime.ToString());
	}

	private void ResetCooldownTime()
	{
		Debug.Log("Cooldown time reset for: " + base.name);
		SetCooldownTime(DateTime.UtcNow);
	}
}
