using UnityEngine;

public class NPC_NavMove_Callback : MonoBehaviour
{
	public GameObject gobj;

	public NPC_NavMove navMove;

	private NpcType npcType;

	private void Awake()
	{
		if (gobj == null)
		{
			gobj = base.gameObject;
			Debug.LogWarning("Set gobj in " + gobj.name);
		}
		if (navMove == null)
		{
			Debug.LogWarning("Set navMove in " + gobj.name);
		}
		npcType = navMove.npcType;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "ObjectArea")
		{
			other.GetComponent<IObjectArea>().OnAreaEnter_Npc();
		}
		else if (other.tag == "Marker" && other.GetComponent<Marker_Trigger>().CheckType(npcType))
		{
			navMove.EnterMarker(other);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "ObjectArea")
		{
			other.GetComponent<IObjectArea>().OnAreaExit_Npc();
		}
	}

	public void EnableCallback(bool isOn)
	{
		gobj.SetActive(isOn);
	}
}
