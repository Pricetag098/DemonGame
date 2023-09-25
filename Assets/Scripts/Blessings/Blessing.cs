using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blessing : ScriptableObject
{
    protected BlessingStatusHandler handler;
    public bool instantEffect = true;

    /// <summary>
    /// Call to apply the effect
    /// </summary>
    /// <param name="handler"></param>
    public void Equip(BlessingStatusHandler handler)
    {
        this.handler = handler;
        OnEquip();
        if (!instantEffect)
        {
            handler.activeBlessings.Add(this);
        }
        else
        {
            OnRemove();
        }
    }

    protected virtual void OnEquip()
    {

    }

    public virtual void Tick()
    {

    }

    public void Remove()
    {
        OnRemove();
        handler.activeBlessings.Remove(this);
    }

    protected virtual void OnRemove()
    {

    }
}
