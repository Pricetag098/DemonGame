using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeInventory : MonoBehaviour
{
    [Header("Kitchen Puzzle")]
    bool hasArm;

    [Header("Kine Puzzle")]
    bool hasKnife;
    bool hasFalseKnife;
    bool hasShovel;

    [Header("Painting Puzzle")]
    bool hasSalt;
    List<GameObject> Paintings;

    private void Start()
    {
        
    }

    public void PickupArm()
    {
        hasArm = true;
        //Anything else
    }

    public void PickupSalt()
    {
        hasSalt = true;
        //Anything else
    }

    public void PickupKnife()
    {
        hasKnife = true;
        //Anything else
    }

    public void PickupFalseKnife()
    {
        hasFalseKnife = true;
        //Anything else
    }

    public void PickupShovel()
    {
        hasShovel = true;
        //Anything else
    }
}
