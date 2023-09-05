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
    public List<Painting> paintings;
    int placeInList = 0;

    [Header("Final Ritual")]
    public GameObject finalRitual;

    private void Start()
    {
        RandomizeList(paintings);
    }

    public void PickupArm()
    {
        hasArm = true;
        //Anything else

        CheckForFinish();
    }

    public void PickupSalt()
    {
        hasSalt = true;
        //Anything else

        CheckForFinish();
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

        CheckForFinish();
    }

    public void PickupFalseKnife()
    {
        hasFalseKnife = true;
        //Anything else
    }

    public void PickupShovel()
    {
        if (hasShovel)
        {
            hasShovel = false;
        }
        else
        {
            hasShovel = true;
        }        //Anything else
    }

    public void CheckForFinish()
    {
        if(hasKnife && hasSalt && hasArm)
        {
            finalRitual.SetActive(true);
        }
    }

    public void CheckPainting(Painting painting)
    {
        if(painting == paintings[placeInList])
        {
            painting.CorrectPainting();
            if(placeInList >= paintings.Count - 1)
            {
                painting.FinishedPuzzle();

                foreach (Painting obj in paintings)
                {
                    obj.SetDisabled();
                }
            }
            else
            {
                placeInList++;
            }
        }
        else
        {
            painting.failSound.Play();

            foreach (Painting obj in paintings)
            {
                obj.FailedPainting();
            }
            placeInList = 0;
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
