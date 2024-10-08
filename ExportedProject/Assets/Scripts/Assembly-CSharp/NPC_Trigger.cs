using UnityEngine;

public class NPC_Trigger : MonoBehaviour
{
	public GameObject gobj;

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
	}

	private void OnTriggerEnter(Collider other)
	{
		switch (other.tag)
		{
		case "Boost":
		{
			Boost component = other.GetComponent<Boost>();
			if (npc.CheckBoostEffectType(component.boostType) && component.NpcAction())
			{
				npc.BoostAction(component.boostType);
			}
			break;
		}
		case "Player":
			npc.PlayerCollision();
			break;
		}
	}

	public void EnableTrigger(bool isOn)
	{
		gobj.SetActive(isOn);
	}
}
