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
    public List<GameObject> paintings;
    int placeInList = 0;

    private void Start()
    {
        RandomizeList(paintings);
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
        if(hasKnife)
        {
            hasKnife = false;
        }
        else
        {
            hasKnife = true;
        }
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

    public void CheckPainting(GameObject painting)
    {
        if(painting == paintings[placeInList])
        {
            placeInList++;
            //painting.GetComponent<Painting>()
        }
    }

    public static void RandomizeList<T>(List<T> list)
    {
        int n = list.Count;
        System.Random random = new System.Random();

        for (int i = n - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);

            // Swap list[i] and list[j]
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
