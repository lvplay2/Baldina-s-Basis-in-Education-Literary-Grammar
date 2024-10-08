using UnityEngine;

public class GoldBallFly : MonoBehaviour
{
	public Boost boost;

	private bool isOnNpc;

	public GameObject gobj;

	public Transform tform;

	public float speed;

	private void Update()
	{
		tform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

	public void InitFly(Vector3 pos, Vector3 forward)
	{
		boost.isActive = false;
		isOnNpc = false;
		tform.forward = forward;
		tform.localPosition = pos + forward * 2f;
		base.enabled = true;
		gobj.SetActive(true);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (isOnNpc)
		{
			return;
		}
		NPC_Trigger component = other.GetComponent<NPC_Trigger>();
		if (component != null && component.npc.npcType != NpcType.Thed)
		{
			if (component.npc.npcType == NpcType.Security)
			{
				return;
			}
			boost.isActive = true;
			isOnNpc = true;
			Sing_Game.This.npc_Thed.punishNpcGold = component.npc;
			tform.parent = component.npc.tform;
			tform.localPosition = Vector3.up;
			Sing_Game.This.npc_Thed.CatchBall();
		}
		else
		{
			gobj.SetActive(false);
		}
		base.enabled = false;
	}
}
