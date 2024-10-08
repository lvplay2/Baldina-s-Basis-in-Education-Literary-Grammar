using System;
using System.Globalization;
using System.Net;
using CompleteProject;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
	public Button button_WatchAds;

	public GameObject go_PanelError;

	public GameObject go_PanelRestart;

	public GameObject go_PanelShop;

	public GameObject go_PanelSuccess;

	public GameObject go_PanelPause;

	public GameObject go_ButtonRestorePurchases;

	public Text text_ErrorNumber;

	public Text text_WatchAdsAwailableCount;

	private Purchaser _purchaser;

	private float _gameTimeScale = 1f;

	private int _goods;

	public static Shop This { get; private set; }

	public int Goods
	{
		get
		{
			return _goods;
		}
		set
		{
			_goods = value;
			PlayerPrefs.SetInt("Shop : Goods", value);
		}
	}

	public int WatchAdsAwailableCount
	{
		get
		{
			return PlayerPrefs.GetInt("Shop : WatchAdsAwailableCount", 0);
		}
		set
		{
			PlayerPrefs.SetInt("Shop : WatchAdsAwailableCount", value);
		}
	}

	public bool NoAds
	{
		get
		{
			bool flag = PlayerPrefs.GetInt("Shop : NoAds", 0) == 1;
			bool flag2 = PlayerPrefs.GetInt("Shop : IsAds", 1) == 0;
			return flag || flag2;
		}
		set
		{
			PlayerPrefs.SetInt("Shop : NoAds", value ? 1 : 0);
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
		_purchaser = base.gameObject.GetComponent<Purchaser>();
		_goods = PlayerPrefs.GetInt("Shop : Goods", 0);
		string @string = PlayerPrefs.GetString("Shop : LastDay", string.Empty);
		if (_CheckIsNewDay())
		{
			WatchAdsAwailableCount = 3;
			PlayerPrefs.SetString("Shop : LastDay", DateTime.Now.ToString());
		}
		text_WatchAdsAwailableCount.text = WatchAdsAwailableCount.ToString();
		button_WatchAds.interactable = WatchAdsAwailableCount > 0;
		go_ButtonRestorePurchases.SetActive(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.WindowsEditor);
	}

	public void MY_DisableAds()
	{
		NoAds = true;
		go_PanelRestart.SetActive(true);
	}

	public void MY_EnableAds()
	{
		NoAds = false;
		go_PanelRestart.SetActive(true);
	}

	public void MY_BuyGoods(int id)
	{
		switch (id)
		{
		case 1:
			Goods += 55;
			break;
		case 2:
			Goods += 165;
			break;
		case 3:
			Goods += 320;
			break;
		case 4:
			Goods += 633;
			break;
		case 5:
			Goods += 1333;
			break;
		case 6:
			Goods += 3780;
			break;
		case 7:
			Goods += 8330;
			break;
		default:
			MY_ShowError("S: 1001");
			return;
		}
		go_PanelSuccess.SetActive(true);
	}

	public void MY_RestorePurchases()
	{
		_purchaser.MY_RestorePurchases();
	}

	public void MY_ShowError(string message)
	{
		text_ErrorNumber.text = message;
		go_PanelError.SetActive(true);
	}

	public void MY_GamePause(bool isPause)
	{
		_gameTimeScale = ((!isPause) ? _gameTimeScale : Time.timeScale);
		Time.timeScale = ((!isPause) ? _gameTimeScale : 0f);
		AudioListener.pause = isPause;
		go_PanelPause.SetActive(isPause);
	}

	public void UA_Open()
	{
		MY_GamePause(true);
		go_PanelShop.SetActive(true);
	}

	public void UA_BuyGoods(int id)
	{
		_purchaser.MY_BuyConsumable(id);
	}

	public void UA_DisableAds()
	{
		switch (Application.platform)
		{
		case RuntimePlatform.IPhonePlayer:
			_purchaser.MY_BuyNonConsumable(0);
			break;
		case RuntimePlatform.Android:
			_purchaser.MY_BuyConsumable(0);
			break;
		case RuntimePlatform.WindowsEditor:
			_purchaser.MY_BuyNonConsumable(0);
			break;
		}
	}

	public void UA_GetForWatchAds()
	{
		if (WatchAdsAwailableCount > 0)
		{
			AdsController.This.MY_ShowRewardedVideo(_WatchAds);
		}
	}

	public void UA_CloseSuccess()
	{
		go_PanelSuccess.SetActive(false);
	}

	public void UA_Close()
	{
		go_PanelShop.SetActive(false);
		go_PanelError.SetActive(false);
		go_PanelRestart.SetActive(false);
		go_PanelSuccess.SetActive(false);
		MY_GamePause(false);
	}

	public void UA_Restart()
	{
		Application.Quit();
	}

	private void _WatchAds()
	{
		Goods += 10;
		WatchAdsAwailableCount--;
		button_WatchAds.interactable = WatchAdsAwailableCount > 0;
		text_WatchAdsAwailableCount.text = WatchAdsAwailableCount.ToString();
	}

	private bool _CheckIsNewDay()
	{
		string @string = PlayerPrefs.GetString("Shop : LastDay", string.Empty);
		DateTime nistTime;
		try
		{
			nistTime = GetNistTime();
		}
		catch
		{
			return false;
		}
		PlayerPrefs.SetString("Shop : LastDay", nistTime.ToString());
		if (@string.Length == 0)
		{
			return true;
		}
		DateTime dateTime = Convert.ToDateTime(@string);
		if ((nistTime - dateTime).TotalDays > 1.0)
		{
			return true;
		}
		return false;
	}

	public static DateTime GetNistTime()
	{
		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.google.com");
		WebResponse response = httpWebRequest.GetResponse();
		string s = response.Headers["date"];
		return DateTime.ParseExact(s, "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);
	}
}
