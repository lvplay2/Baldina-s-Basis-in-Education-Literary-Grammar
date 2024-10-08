using UnityEngine;
using UnityEngine.UI;

public class ExamplesTable : MonoBehaviour
{
	public GameObject gobj;

	public Text questionText;

	public Text answerText;

	private int examplesCount;

	private string _girlExampleQuestion = "Help me to solve an example:\n";

	private string _currentAnswer;

	[SerializeField]
	private int _currentExample;

	private void Start()
	{
		examplesCount = GameplayManager.This.girl_exampleCount;
	}

	public void EnableTable(bool isOn)
	{
		gobj.SetActive(isOn);
		GameplayManager.This.MY_SwitchCursor(isOn);
		if (isOn)
		{
			_currentExample = 0;
			NextExample();
		}
	}

	private string GenerateExample()
	{
		int num = Random.Range(-9, 10);
		int num2 = Random.Range(-9, 10);
		_currentAnswer = (num + num2).ToString();
		string girlExampleQuestion = _girlExampleQuestion;
		girlExampleQuestion += num;
		if (num2 < 0)
		{
			return girlExampleQuestion + num2;
		}
		return girlExampleQuestion + "+" + num2;
	}

	private void NextExample()
	{
		questionText.text = GenerateExample();
		answerText.text = string.Empty;
	}

	public void UA_ButtonNumber(int id)
	{
		answerText.text += id;
	}

	public void UA_ButtonEnter()
	{
		if (answerText.text.Length <= 0)
		{
			return;
		}
		if (answerText.text == _currentAnswer)
		{
			_currentExample++;
			if (_currentExample < examplesCount)
			{
				NextExample();
			}
			else
			{
				Sing_Game.This.npc_Girl.PlayerAction();
			}
		}
		else
		{
			NextExample();
		}
	}

	public void UA_ButtonClear(bool isMinus)
	{
		if (isMinus)
		{
			answerText.text += "-";
		}
		else
		{
			answerText.text = string.Empty;
		}
	}
}
