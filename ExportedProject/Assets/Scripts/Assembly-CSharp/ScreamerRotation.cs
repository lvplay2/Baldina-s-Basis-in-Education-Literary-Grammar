using UnityEngine;
using UnityEngine.AI;

public class ScreamerRotation : MonoBehaviour
{
	public Transform player_tform;

	public Transform baldina_tform;

	public GameObject baldina_sprite;

	public Transform camera_tform;

	public Transform screamerPoint;

	public Sprite spriteScreamer;

	public GameObject audioScream;

	public GameObject[] toDeactiveObjects;

	public float speed;

	public float devY = 1f;

	public float devZ = 1f;

	private int _maxY;

	private int _minY;

	private int _maxZ;

	private int _minZ;

	private void Start()
	{
		baldina_tform.GetComponent<NavMeshAgent>().enabled = false;
		baldina_sprite.GetComponent<Animator>().enabled = false;
		baldina_sprite.GetComponent<SpriteRenderer>().sprite = spriteScreamer;
		baldina_tform.parent = camera_tform.transform;
		baldina_tform.transform.position = screamerPoint.position;
		baldina_tform.transform.rotation = Quaternion.Euler(screamerPoint.rotation.eulerAngles.x, screamerPoint.rotation.eulerAngles.y, screamerPoint.rotation.eulerAngles.z);
		baldina_tform.parent = null;
		_maxY = (int)player_tform.rotation.eulerAngles.y + (int)devY;
		_minY = (int)player_tform.rotation.eulerAngles.y - (int)devY;
		_maxZ = (int)player_tform.rotation.eulerAngles.z + (int)devZ;
		_minZ = (int)player_tform.rotation.eulerAngles.z - (int)devZ;
		for (int i = 0; i < toDeactiveObjects.Length; i++)
		{
			toDeactiveObjects[i].SetActive(false);
		}
		audioScream.SetActive(true);
		Invoke("_Restart", 3f);
	}

	private void Update()
	{
		_RotateByY();
		_RotateByZ();
	}

	private void _RotateByY()
	{
		float num = Random.Range(0f, devY);
		int num2 = Random.Range(0, 2);
		if (num2 == 1)
		{
			if (player_tform.rotation.eulerAngles.y + num > (float)_maxY)
			{
				num = (float)_maxY - player_tform.rotation.eulerAngles.y;
			}
		}
		else if (player_tform.rotation.eulerAngles.y - num < (float)_minY)
		{
			num = player_tform.rotation.eulerAngles.y - (float)_minY;
		}
		num2 = ((num2 == 1) ? 1 : (-1));
		num *= (float)num2;
		player_tform.Rotate(0f, num, 0f);
	}

	private void _RotateByZ()
	{
		float num = Random.Range(0f, devZ);
		int num2 = Random.Range(0, 2);
		if (num2 == 1)
		{
			if (player_tform.rotation.eulerAngles.z + num > (float)_maxZ)
			{
				num = (float)_maxZ - player_tform.rotation.eulerAngles.z;
			}
		}
		else if (player_tform.rotation.eulerAngles.z - num < (float)_minZ)
		{
			num = player_tform.rotation.eulerAngles.z - (float)_minZ;
		}
		num2 = ((num2 == 1) ? 1 : (-1));
		num *= (float)num2;
		player_tform.Rotate(0f, 0f, num);
	}

	private void _Restart()
	{
		MultiSceneManager.This.LoadSceneAsync("Lose");
	}
}
