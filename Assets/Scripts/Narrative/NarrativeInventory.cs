using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeInventory : MonoBehaviour
{
    [Header("Kitchen Puzzle")]
    [SerializeField] bool hasArm;

    [Header("Kine Puzzle")]
    [SerializeField] bool hasKnife;
    [SerializeField] bool hasFalseKnife;
    [SerializeField] bool hasShovel;

    [Header("Painting Puzzle")]
    [SerializeField] bool hasSalt;
    public List<Painting> paintings;
    int placeInList = 0;

    [Header("Obelisk Puzzle")]
    [SerializeField] bool finishedObelisks;

    [Header("Final Ritual")]
    public GameObject divineSmite;

    private void Start()
    {
        RandomizeList(paintings);
    }

    public void AllObelisks()
    {
        finishedObelisks = true;

        CheckForFinish();
    }

    public void PickupArm()
    {
        hasArm = true;

        CheckForFinish();
    }

    public void PickupSalt()
    {
        hasSalt = true;

        CheckForFinish();
    }

    public void PickupKnife()
    {
        if(hasKnife)
        {
            if (hasFalseKnife)
            {
                hasFalseKnife = false;
            }
            else
            {
                hasKnife = false;
            }
        }
        else
        {
            hasKnife = true;
        }

        CheckForFinish();
    }

    public void PickupFalseKnife()
    {
        hasFalseKnife = true;
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
        }        
    }

    public void CheckForFinish()
    {
        if(hasKnife && hasSalt && hasArm && finishedObelisks)
        {
            divineSmite.SetActive(true);
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
