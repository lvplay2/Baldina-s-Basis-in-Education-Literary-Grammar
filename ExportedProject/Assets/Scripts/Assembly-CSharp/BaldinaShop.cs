using System;
using UnityEngine;
using UnityEngine.UI;

public class BaldinaShop : MonoBehaviour
{
	public enum ChangeButton
	{
		BuyItems = 0,
		BuyBoosts = 1,
		DisableAds = 2,
		DisableCharacters = 3,
		BuyGoods = 4,
		ToMain = 5,
		Back = 6
	}

	[Serializable]
	public enum BuyItems
	{
		Book = 0,
		Burger = 1,
		Answers = 2,
		Lockpick = 3,
		Newspaper = 4,
		Boots = 5,
		Key = 6,
		Mirror = 7,
		GoldBall = 8
	}

	[Serializable]
	public enum BuyDisableCharacters
	{
		Girl = 0,
		Director = 1,
		Cleaner = 2,
		Bully = 3,
		Security = 4,
		Queen = 5,
		Thed = 6
	}

	[Serializable]
	public enum BuyBoosts
	{
		SlowBaldina = 0,
		EndlessStamina = 1,
		ImmortalityMode = 2,
		UnlimitedAll = 3
	}

	private struct SellItems
	{
		private BuyItems _item;

		private int _price;

		public Text priceText;

		private int _haveCount;

		public Text haveCountText;

		public Text haveCountSpendText;

		public BuyItems item
		{
			get
			{
				return _item;
			}
			set
			{
				_item = value;
				_haveCount = PlayerPrefs.GetInt("Shop-Good-" + _item);
				haveCountText.text = _haveCount.ToString();
				haveCountSpendText.text = _haveCount.ToString();
			}
		}

		public int price
		{
			get
			{
				return _price;
			}
			set
			{
				_price = value;
				priceText.text = _price.ToString();
			}
		}

		public int haveCount
		{
			get
			{
				if (This.isUnlimited)
				{
					return 1;
				}
				return _haveCount;
			}
			set
			{
				if (!This.isUnlimited)
				{
					_haveCount = value;
					PlayerPrefs.SetInt("Shop-Good-" + item, _haveCount);
					haveCountText.text = _haveCount.ToString();
					haveCountSpendText.text = _haveCount.ToString();
				}
			}
		}
	}

	private struct SellDisableCharacters
	{
		private BuyDisableCharacters _character;

		private int _price;

		public Text priceText;

		private int _haveCount;

		public Text haveCountText;

		public Text haveCountSpendText;

		public BuyDisableCharacters character
		{
			get
			{
				return _character;
			}
			set
			{
				_character = value;
				_haveCount = PlayerPrefs.GetInt("Shop-DisableCharacter-" + _character);
				haveCountText.text = _haveCount.ToString();
				haveCountSpendText.text = _haveCount.ToString();
			}
		}

		public int price
		{
			get
			{
				return _price;
			}
			set
			{
				_price = value;
				priceText.text = _price.ToString();
			}
		}

		public int haveCount
		{
			get
			{
				if (This.isUnlimited)
				{
					return 1;
				}
				return _haveCount;
			}
			set
			{
				if (!This.isUnlimited)
				{
					_haveCount = value;
					PlayerPrefs.SetInt("Shop-DisableCharacter-" + character, _haveCount);
					haveCountText.text = _haveCount.ToString();
					haveCountSpendText.text = _haveCount.ToString();
				}
			}
		}
	}

	private struct SellBoosts
	{
		private BuyBoosts _boost;

		private int _price;

		public Text priceText;

		private bool _isBuy;

		public Button buyButton;

		public GameObject yesImage;

		public GameObject noImage;

		public bool isActive;

		public BuyBoosts boost
		{
			get
			{
				return _boost;
			}
			set
			{
				_boost = value;
				_isBuy = PlayerPrefs.GetInt("Shop-Boost-" + _boost) == 1;
				buyButton.enabled = !_isBuy;
				yesImage.SetActive(isBuy);
				noImage.SetActive(!isBuy);
			}
		}

		public int price
		{
			get
			{
				return _price;
			}
			set
			{
				_price = value;
				priceText.text = _price.ToString();
			}
		}

		public bool isBuy
		{
			get
			{
				return _isBuy;
			}
			set
			{
				_isBuy = value;
				PlayerPrefs.SetInt("Shop-Boost-" + _boost, _isBuy ? 1 : 0);
				buyButton.enabled = !_isBuy;
				yesImage.SetActive(isBuy);
				noImage.SetActive(!isBuy);
			}
		}
	}

	public GameObject canvasBaldinaShop;

	public GameObject canvasBaldinaShopSpend;

	public GameObject canvasBaldinaShopSpendBoosts;

	public GameObject canvasShopMain;

	public GameObject canvasShopItems;

	public GameObject canvasShopBoosts;

	public GameObject canvasShopDisableCharacters;

	public GameObject canvasShopNoMoney;

	public Transform planeItemsBuy;

	public Transform planeItemsHave;

	public Transform planeItemsHaveSpend;

	public Transform planeDisableCharactersBuy;

	public Transform planeDisableCharactersHave;

	public Transform planeDisableCharactersHaveSpend;

	public Transform planeBoostBuy;

	public Transform planeBoostHave;

	public Transform planeBoostHaveSpend;

	public Animator interfacePlaceAnim;

	public Image[] interfacePlaces;

	public bool isUnlimited;

	private int[] itemPrices = new int[9] { 30, 15, 27, 12, 15, 30, 25, 21, 21 };

	private int[] disableCharactersPrices = new int[7] { 299, 199, 239, 69, 139, 169, 99 };

	private int[] boostsPrices = new int[4] { 1999, 999, 4000, 900 };

	private int skipPrice = 30;

	public Text currentGoodText;

	private int _currentGoodCount;

	private int _currentSkipCount;

	public Text skipCountText;

	public Text skipPriceText;

	private SellItems[] sellItems;

	private SellDisableCharacters[] sellDisableCharacters;

	private SellBoosts[] sellBoost;

	public static BaldinaShop This { get; private set; }

	private int currentGoodCount
	{
		get
		{
			return _currentGoodCount;
		}
		set
		{
			_currentGoodCount = value;
			Shop.This.Goods = _currentGoodCount;
			currentGoodText.text = _currentGoodCount.ToString();
		}
	}

	public int currentSkipCount
	{
		get
		{
			return _currentSkipCount;
		}
		set
		{
			_currentSkipCount = value;
			PlayerPrefs.SetInt("Shop : SkipsPunishment", _currentSkipCount);
			skipCountText.text = _currentSkipCount.ToString();
		}
	}

	private void Awake()
	{
		This = this;
		Init();
	}

	private void Start()
	{
		UpdateVaruables();
	}

	private void UpdateVaruables()
	{
		_currentGoodCount = Shop.This.Goods;
		currentGoodText.text = currentGoodCount.ToString();
		_currentSkipCount = PlayerPrefs.GetInt("Shop : SkipsPunishment");
		skipCountText.text = currentSkipCount.ToString();
		skipPriceText.text = skipPrice.ToString();
	}

	private void Init()
	{
		int num = Enum.GetNames(typeof(BuyBoosts)).Length;
		sellBoost = new SellBoosts[num];
		for (int i = 0; i < num; i++)
		{
			sellBoost[i].priceText = planeBoostBuy.GetChild(i).GetChild(0).GetComponent<Text>();
			sellBoost[i].buyButton = planeBoostBuy.GetChild(i).GetComponent<Button>();
			sellBoost[i].yesImage = planeBoostHave.GetChild(i).GetChild(0).gameObject;
			sellBoost[i].noImage = planeBoostHave.GetChild(i).GetChild(1).gameObject;
			sellBoost[i].boost = (BuyBoosts)i;
			sellBoost[i].price = boostsPrices[i];
		}
		if (sellBoost[3].isBuy)
		{
			isUnlimited = true;
			for (int j = 0; j < 3; j++)
			{
				planeBoostHaveSpend.GetChild(j).GetComponent<Toggle>().interactable = true;
				planeBoostHaveSpend.GetChild(j + 3).gameObject.SetActive(false);
			}
		}
		int num2 = Enum.GetNames(typeof(BuyItems)).Length;
		sellItems = new SellItems[num2];
		for (int k = 0; k < num2; k++)
		{
			if (!isUnlimited)
			{
				planeItemsHaveSpend.GetChild(k).GetChild(1).gameObject.SetActive(true);
			}
			sellItems[k].priceText = planeItemsBuy.GetChild(k).GetChild(0).GetComponent<Text>();
			sellItems[k].haveCountText = planeItemsHave.GetChild(k).GetChild(1).GetChild(0)
				.GetComponent<Text>();
			sellItems[k].haveCountSpendText = planeItemsHaveSpend.GetChild(k).GetChild(1).GetChild(0)
				.GetComponent<Text>();
			sellItems[k].item = (BuyItems)k;
			sellItems[k].price = itemPrices[k];
		}
		int num3 = Enum.GetNames(typeof(BuyDisableCharacters)).Length;
		sellDisableCharacters = new SellDisableCharacters[num3];
		for (int l = 0; l < num3; l++)
		{
			if (!isUnlimited)
			{
				planeDisableCharactersHaveSpend.GetChild(l).GetChild(1).gameObject.SetActive(true);
			}
			sellDisableCharacters[l].priceText = planeDisableCharactersBuy.GetChild(l).GetChild(0).GetComponent<Text>();
			sellDisableCharacters[l].haveCountText = planeDisableCharactersHave.GetChild(l).GetChild(1).GetChild(0)
				.GetComponent<Text>();
			sellDisableCharacters[l].haveCountSpendText = planeDisableCharactersHaveSpend.GetChild(l).GetChild(1).GetChild(0)
				.GetComponent<Text>();
			sellDisableCharacters[l].character = (BuyDisableCharacters)l;
			sellDisableCharacters[l].price = disableCharactersPrices[l];
		}
		for (int m = 0; m < 3; m++)
		{
			planeBoostHaveSpend.GetChild(m).GetComponent<Toggle>().interactable = sellBoost[m].isBuy;
			planeBoostHaveSpend.GetChild(m + 3).gameObject.SetActive(!sellBoost[m].isBuy);
		}
	}

	public void UA_Open(bool isOn = true)
	{
		UpdateVaruables();
		if (!isUnlimited || !isOn)
		{
			canvasBaldinaShop.SetActive(isOn);
			UA_ChangeButton(5);
		}
	}

	public void UA_OpenGame(bool isOn = true)
	{
		canvasBaldinaShopSpend.SetActive(isOn);
		if (isOn)
		{
			interfacePlaces[0].sprite = Sing_Game.This.gameCanvas.boostImages[0].sprite;
			interfacePlaces[1].sprite = Sing_Game.This.gameCanvas.boostImages[1].sprite;
			interfacePlaces[2].sprite = Sing_Game.This.gameCanvas.boostImages[2].sprite;
		}
	}

	public void UA_OpenBoostsGame(bool isOn = true)
	{
		UpdateVaruables();
		canvasBaldinaShopSpendBoosts.SetActive(isOn);
		if (isOn)
		{
			for (int i = 0; i < 3; i++)
			{
				planeBoostHaveSpend.GetChild(i).GetComponent<Toggle>().isOn = sellBoost[i].isActive;
			}
		}
	}

	public void UA_StartGame()
	{
		UA_OpenBoostsGame(false);
		Sing_Menu.This.canvasMenu.EnableLoadingCanvas();
		MultiSceneManager.This.LoadSceneAsync("Game");
	}

	public void UA_BuyBoostOpen()
	{
		UA_Open();
		UA_ChangeButton(1);
	}

	public void UA_Close()
	{
		UA_ChangeButton(5);
		UA_Open(false);
		UA_OpenGame(false);
		UA_OpenBoostsGame(false);
	}

	public void UA_ChangeButton(int id)
	{
		canvasShopMain.SetActive(false);
		canvasShopItems.SetActive(false);
		canvasShopBoosts.SetActive(false);
		canvasShopDisableCharacters.SetActive(false);
		switch ((ChangeButton)id)
		{
		case ChangeButton.BuyItems:
			canvasShopItems.SetActive(true);
			break;
		case ChangeButton.BuyBoosts:
			canvasShopBoosts.SetActive(true);
			break;
		case ChangeButton.DisableAds:
			canvasShopMain.SetActive(true);
			Shop.This.UA_Open();
			break;
		case ChangeButton.DisableCharacters:
			canvasShopDisableCharacters.SetActive(true);
			break;
		case ChangeButton.BuyGoods:
			canvasShopMain.SetActive(true);
			Shop.This.UA_Open();
			break;
		case ChangeButton.ToMain:
			canvasShopMain.SetActive(true);
			break;
		case ChangeButton.Back:
			UA_Open(false);
			break;
		}
	}

	public void UA_BuySkip()
	{
		if (currentGoodCount >= skipPrice)
		{
			currentGoodCount -= skipPrice;
			currentSkipCount++;
			Sing_Menu.This.canvasMenu.UpdateVariables();
		}
	}

	public void UA_BuyItem(int id)
	{
		if (currentGoodCount >= sellItems[id].price)
		{
			currentGoodCount -= sellItems[id].price;
			sellItems[id].haveCount++;
		}
		else
		{
			canvasShopNoMoney.SetActive(true);
		}
	}

	public void UA_BuyCharacter(int id)
	{
		if (currentGoodCount >= sellDisableCharacters[id].price)
		{
			currentGoodCount -= sellDisableCharacters[id].price;
			sellDisableCharacters[id].haveCount++;
		}
		else
		{
			canvasShopNoMoney.SetActive(true);
		}
	}

	public void UA_BuyBoosts(int id)
	{
		if (currentGoodCount >= sellBoost[id].price)
		{
			currentGoodCount -= sellBoost[id].price;
			sellBoost[id].isBuy = true;
			if (id < 3)
			{
				planeBoostHaveSpend.GetChild(id).GetComponent<Toggle>().interactable = true;
				planeBoostHaveSpend.GetChild(id + 3).gameObject.SetActive(false);
				return;
			}
			isUnlimited = true;
			UA_ChangeButton(5);
			UA_Open(false);
			Sing_Menu.This.canvasMenu.shopButton.SetActive(false);
			for (int i = 0; i < 3; i++)
			{
				sellBoost[i].isBuy = true;
				planeBoostHaveSpend.GetChild(i).GetComponent<Toggle>().interactable = true;
				planeBoostHaveSpend.GetChild(i + 3).gameObject.SetActive(false);
			}
			int num = Enum.GetNames(typeof(BuyItems)).Length;
			for (int j = 0; j < num; j++)
			{
				planeItemsHaveSpend.GetChild(j).GetChild(1).gameObject.SetActive(false);
			}
			int num2 = Enum.GetNames(typeof(BuyDisableCharacters)).Length;
			for (int k = 0; k < num2; k++)
			{
				planeDisableCharactersHaveSpend.GetChild(k).GetChild(1).gameObject.SetActive(false);
			}
		}
		else
		{
			canvasShopNoMoney.SetActive(true);
		}
	}

	public void UA_NoMoney(bool isBuy)
	{
		if (isBuy)
		{
			Shop.This.UA_Open();
		}
		canvasShopNoMoney.SetActive(false);
	}

	public void UA_InterfacePlaceClick(int id)
	{
		interfacePlaceAnim.SetInteger("ChooseId", id);
	}

	public void UA_UseItem(int id)
	{
		if (sellItems[id].haveCount > 0)
		{
			sellItems[id].haveCount--;
			int integer = interfacePlaceAnim.GetInteger("ChooseId");
			switch ((BuyItems)id)
			{
			case BuyItems.Book:
				Sing_Game.This.gameCanvas.AddBoostShop(BoostType.Book, integer);
				break;
			case BuyItems.Burger:
				Sing_Game.This.gameCanvas.AddBoostShop(BoostType.Burger, integer);
				break;
			case BuyItems.Answers:
				Sing_Game.This.gameCanvas.AddBoostShop(BoostType.Examples, integer);
				break;
			case BuyItems.Lockpick:
				Sing_Game.This.gameCanvas.AddBoostShop(BoostType.Skelekey, integer);
				break;
			case BuyItems.Newspaper:
				Sing_Game.This.gameCanvas.AddBoostShop(BoostType.Newspaper, integer);
				break;
			case BuyItems.Boots:
				Sing_Game.This.gameCanvas.AddBoostShop(BoostType.Boots, integer);
				break;
			case BuyItems.Key:
				Sing_Game.This.gameCanvas.AddBoostShop(BoostType.Key, integer);
				break;
			case BuyItems.Mirror:
				Sing_Game.This.gameCanvas.AddBoostShop(BoostType.Mirror, integer);
				break;
			case BuyItems.GoldBall:
				Sing_Game.This.gameCanvas.AddBoostShop(BoostType.GoldBall, integer);
				break;
			}
			UA_OpenGame();
		}
	}

	public void UA_UseDisableCharacter(int id)
	{
		if (sellDisableCharacters[id].haveCount > 0 && Sing_Game.This.npcCharacters[id].gobj.activeSelf)
		{
			sellDisableCharacters[id].haveCount--;
			Sing_Game.This.npcCharacters[id].DisableInGame();
		}
	}

	public void UA_UseBoost(bool isOn, int id)
	{
	}

	public void UA_UseBoostSlow(bool isOn)
	{
		sellBoost[0].isActive = isOn;
	}

	public void UA_UseBoostStamina(bool isOn)
	{
		sellBoost[1].isActive = isOn;
	}

	public void UA_UseBoostImmortal(bool isOn)
	{
		sellBoost[2].isActive = isOn;
		Physics.IgnoreLayerCollision(12, 13, isOn);
		Physics.IgnoreLayerCollision(16, 13, isOn);
	}

	public bool CheckBoost(BuyBoosts type)
	{
		return sellBoost[(int)type].isActive;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			ClearPurchases();
		}
	}

	private void ClearPurchases()
	{
		for (int i = 0; i < 9; i++)
		{
			PlayerPrefs.SetInt("Shop-Good-" + (BuyItems)i, 0);
		}
		for (int j = 0; j < 7; j++)
		{
			PlayerPrefs.SetInt("Shop-DisableCharacter-" + (BuyDisableCharacters)j, 0);
		}
		for (int k = 0; k < 4; k++)
		{
			PlayerPrefs.SetInt("Shop-Boost-" + (BuyBoosts)k, 0);
		}
	}
}
