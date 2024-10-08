using System;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Tutorial : Canvas_Main
{
	[Serializable]
	public struct CharactersInfo
	{
		public string name;

		public string description;

		public Sprite sprite;
	}

	[Serializable]
	public struct ItemsInfo
	{
		public string name;

		public string description;

		public Sprite sprite;
	}

	public CharactersInfo[] characters;

	public ItemsInfo[] items;

	public GameObject infoObject;

	public Image infoImage;

	public Text infoName;

	public Text infoDescription;

	public void ButtonOk()
	{
		if (GameplayManager.This.isChristmasIntro)
		{
			MultiSceneManager.This.LoadScene("ChristmasIntro");
		}
		else
		{
			MultiSceneManager.This.LoadScene("Menu");
		}
	}

	public void CharacterInfoClick(int _id)
	{
		infoObject.SetActive(true);
		infoImage.sprite = characters[_id].sprite;
		infoName.text = characters[_id].name;
		infoDescription.text = characters[_id].description;
	}

	public void InfoClose()
	{
		infoObject.SetActive(false);
	}

	public void ItemsInfoClick(int _id)
	{
		infoObject.SetActive(true);
		infoImage.sprite = items[_id].sprite;
		infoName.text = items[_id].name;
		infoDescription.text = items[_id].description;
	}
}
