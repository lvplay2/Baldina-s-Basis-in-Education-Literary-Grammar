using UnityEngine;
using UnityEngine.UI;

public class TextYouHaveCount : MonoBehaviour
{
	private Text _textYouHaveCount;

	private void Start()
	{
		_textYouHaveCount = GetComponent<Text>();
	}

	private void Update()
	{
		_textYouHaveCount.text = Shop.This.Goods.ToString();
	}
}
