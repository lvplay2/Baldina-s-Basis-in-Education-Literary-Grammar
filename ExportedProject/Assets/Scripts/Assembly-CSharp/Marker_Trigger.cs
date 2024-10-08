using UnityEngine;

public class Marker_Trigger : MonoBehaviour
{
	public NpcType npcType;

	public bool CheckType(NpcType _type)
	{
		return _type == npcType;
	}
}
