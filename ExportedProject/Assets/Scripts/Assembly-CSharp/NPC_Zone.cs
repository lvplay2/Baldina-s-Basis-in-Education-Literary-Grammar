using UnityEngine;

public class NPC_Zone : MonoBehaviour
{
	public GameObject gobj;

	public SphereCollider sphereCollider;

	public NPC_Main npc;

	private void Awake()
	{
		if (gobj == null)
		{
			gobj = base.gameObject;
			Debug.LogWarning("Set gobj in " + gobj.name);
		}
		if (npc == null)
		{
			Debug.LogWarning("Set npc in " + gobj.name);
		}
		if (sphereCollider == null)
		{
			sphereCollider = GetComponent<SphereCollider>();
			Debug.LogWarning("Set npc in " + gobj.name);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			npc.EnterZone();
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player")
		{
			npc.StayZone();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			npc.ExitZone();
		}
	}

	public void EnableZone(bool isOn)
	{
		gobj.SetActive(isOn);
	}
}
