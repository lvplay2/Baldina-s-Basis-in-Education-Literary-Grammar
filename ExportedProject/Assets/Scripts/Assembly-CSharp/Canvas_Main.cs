using UnityEngine;

public class Canvas_Main : MonoBehaviour
{
	public virtual void SetPause(bool _isOn)
	{
	}

	public virtual void AddBoost(BoostType _type)
	{
	}

	public virtual void NextChooseBoost(bool _next)
	{
	}

	public virtual void ChooseBoost(int _id)
	{
	}

	public virtual void ChangeBoostName(int _id)
	{
	}

	public virtual void UseBoost()
	{
	}

	public virtual void PunishTimerEnable(bool isOn, int time = 0)
	{
	}
}
