using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "Ability")]
public abstract class Ability : ScriptableObject
{
    public GameObject a;
    public virtual void Tick(bool active)
	{

	}

	public virtual void EnableInputs()
	{

	}
	public virtual void DisableInputs()
	{

	}
	

}
