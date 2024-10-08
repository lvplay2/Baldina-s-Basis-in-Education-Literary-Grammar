using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiSceneManager : MonoBehaviour
{
	[Header("Check if Debug")]
	public bool isDebug;

	private IEnumerator asyncLoadingCoroutine;

	public static MultiSceneManager This { get; private set; }

	private void Awake()
	{
		This = this;
		Object.DontDestroyOnLoad(this);
		if (isDebug && !GetComponent<Debug_Temp>())
		{
			base.gameObject.AddComponent<Debug_Temp>();
		}
		SceneManager.sceneLoaded += _SceneLoaded;
	}

	public void LoadScene(string _name)
	{
		DisableShops();
		SceneManager.LoadScene(_name);
	}

	public void LoadSceneAsync(string _name)
	{
		DisableShops();
		if (asyncLoadingCoroutine != null)
		{
			StopCoroutine(asyncLoadingCoroutine);
		}
		asyncLoadingCoroutine = LoadindSceneAsync(_name);
		StartCoroutine(asyncLoadingCoroutine);
	}

	private void DisableShops()
	{
		Shop.This.UA_Close();
		BaldinaShop.This.UA_Close();
	}

	private IEnumerator LoadindSceneAsync(string _name)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_name);
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}

	private void _SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		Time.timeScale = 1f;
		string text = scene.name;
		if (text != null && text == "Game")
		{
			Sing_Game.This.currentPlatformInput.InitInput();
		}
	}
}
