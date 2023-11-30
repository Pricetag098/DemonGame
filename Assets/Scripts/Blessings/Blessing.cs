using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blessing : ScriptableObject
{
    protected BlessingStatusHandler handler;
    public bool instantEffect = true;
    public string blessingName = "";
    public string blessingFontRef = null;


    /// <summary>
    /// Call to apply the effect
    /// </summary>
    /// <param name="handler"></param>
    public void Equip(BlessingStatusHandler handler)
    {
        this.handler = handler;
        handler.DisplayBlessing(this);
        OnEquip();
        if (!instantEffect)
        {
            foreach(Blessing b in handler.activeBlessings)
            {
                if(b.GetType() == GetType())
                {
                    b.ReEquip();
                    return;
                }
            }
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
    public virtual void ReEquip()
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
