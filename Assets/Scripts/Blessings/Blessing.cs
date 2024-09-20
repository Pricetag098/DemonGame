using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Blessing : ScriptableObject
{
    protected BlessingStatusHandler handler;
    public bool instantEffect = true;
    public string blessingName = "";
    public string blessingFontRef = null;
    public TMP_FontAsset blessingFontAsset;
    public AudioClip pickupSound;

    /// <summary>
    /// Call to apply the effect
    /// </summary>
    /// <param name="handler"></param>
    public void Equip(BlessingStatusHandler handler)
    {
        this.handler = handler;
        
        
        if (!instantEffect)
        {
            foreach(Blessing b in handler.activeBlessings)
            {
                if(b.GetType() == GetType())
                {
                    b.ReEquip();
                    Destroy(this);
                    return;
                }
            }
            OnEquip();
            handler.DisplayBlessing(this);
            handler.activeBlessings.Add(this);
        }
        else
        {
            OnEquip();
            handler.DisplayBlessing(this);
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
        Destroy(this);

    }

    protected virtual void OnRemove()
    {

    }
}
