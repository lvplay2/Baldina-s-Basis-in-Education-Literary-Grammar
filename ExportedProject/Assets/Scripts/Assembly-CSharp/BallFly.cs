using UnityEngine;

public class BallFly : MonoBehaviour
{
	public Boost boost;

	public GameObject go;

	public Transform tform;

	public float speed;

	private void Update()
	{
		tform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

	public void InitFly(Vector3 pos, Vector3 forward)
	{
		boost.isActive = true;
		tform.forward = forward;
		tform.localPosition = pos + forward * 2f;
		base.enabled = true;
		go.SetActive(true);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!boost.isActive)
		{
			return;
		}
		NPC_Trigger component = other.GetComponent<NPC_Trigger>();
		if (component != null && component.npc.npcType != NpcType.Thed)
		{
			if (component.npc.npcType != NpcType.Security)
			{
				Sing_Game.This.npc_Thed.punishNpc = component.npc;
				tform.parent = component.npc.tform;
				tform.localPosition = Vector3.up;
				base.enabled = false;
			}
		}
		else if (other.tag != "Player")
		{
			base.enabled = false;
		}
	}
}
