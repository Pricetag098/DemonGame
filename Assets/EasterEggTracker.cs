using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EasterEggTracker : MonoBehaviour
{
    [Header("Bell Step")]
    [SerializeField] float timeBetweenBells;
    [SerializeField] int bellsToShoot;
    [SerializeField] List<GameObject> bells;
    [SerializeField] List<GameObject> bellCompleteObjects;
    [SerializeField] GameObject oldSky;
    [SerializeField] SoundPlayer bellDoneSound;

    [SerializeField] bool bellsDone;
    int bellsShot;
    float bellTimer;
    bool shotBell;

    [Header("Knife Step")]
    [SerializeField] GameObject shovel;
    [SerializeField] GameObject finalKnife;

    [SerializeField] bool hasKnife;

    [Header("Painting Step")]
    [SerializeField] List<GameObject> paintings;
    [SerializeField] List<GameObject> paintingsCompleteObjects;
    [SerializeField] SoundPlayer paintingDoneSound;

    int paintingInteracted;

    [SerializeField] bool paintingsDone;

    [SerializeField] TextMeshProUGUI textMeshProUGUI;

    [Header("Skull Step")]
    [SerializeField] GameObject skullInteractable;
    [SerializeField] GameObject finalSkull;


    [SerializeField] bool hasSkull;

    [Header("Final Ritual")]
    [SerializeField] GameObject placeInteractable;
    [SerializeField] GameObject ritualInteractable;
    [SerializeField] SoundPlayer placeSound;

    private static System.Random rng = new System.Random();

    private void Start()
    {
        RandomizeListInPlace(paintings);

        string first = null;
        string second = null;
        string third = null;
        string fourth = null;

        for (int i = 0; i < paintings.Count; i++)
        {
            switch (i)
            {
                case 0:
                    first = paintings[i].GetComponent<painting>().sigilLetter;
                break;
                case 1:
                    second = paintings[i].GetComponent<painting>().sigilLetter;
                    break;
                case 2:
                    third = paintings[i].GetComponent<painting>().sigilLetter;
                    break;
                case 3:
                    fourth = paintings[i].GetComponent<painting>().sigilLetter;
                    break;
            }
        }

        textMeshProUGUI.text = first + second + third + fourth;
    }

    [ContextMenu("Enable Easter Egg")]
    public void EnableEasterEgg()
    {
        shovel.SetActive(true);

        foreach (GameObject bell in bells)
        {
            bell.SetActive(true);
        }

        foreach (GameObject painting in paintings)
        {
            painting.SetActive(true);
        }

        skullInteractable.SetActive(true);
    }

    [ContextMenu("Complete Easter Egg")]
    public void CompleteEasterEgg()
    {
        ComepletedPaintingPuzzle();
        CompletedBells();
        PickupKnife();
        PickupSkull();
    }

    public void ComepletedPaintingPuzzle()
    {
        paintingsDone = true;

        paintingDoneSound.Play();

        foreach (GameObject obj in paintingsCompleteObjects)
        {
            obj.SetActive(true);
        }

        CheckComplete();
    }

    public void InteractWithPainting(GameObject paintingObj)
    {
        painting painting = paintingObj.GetComponent<painting>(); 
        if (paintingObj == paintings[paintingInteracted])
        {
            paintingInteracted++;
            painting.PaintingGood();
            if (paintingInteracted == paintings.Count)
            {
                ComepletedPaintingPuzzle();
            }
        }
        else
        {
            paintingInteracted = 0;
            painting.PaintingBad();

            foreach (GameObject obj in paintings)
            {
                obj.GetComponent<painting>().ResetPainting();
            }
        }
    }

    void CheckComplete()
    {
        if (paintingsDone)
        {
            if (bellsDone)
            {
                placeInteractable.SetActive(true);
            }
        }
    }

    public void TryPlaceItem()
    {
        if (finalSkull.activeInHierarchy)
        {
            finalKnife.SetActive(true);
            ritualInteractable.SetActive(true);
            placeInteractable.SetActive(false);
            placeSound.Play();
        }
        if (hasSkull)
        {
            finalSkull.SetActive(true);
            placeSound.Play();
        }
    }

    public void PickupSkull()
    {
        hasSkull = true;
    }

    public void CompletedBells()
    {
        bellsDone = true;
        shotBell = false;

        bellDoneSound.Play();

        foreach(GameObject bell in bells)
        {
            bell.SetActive(false);
        }

        foreach (GameObject obj in bellCompleteObjects)
        {
            if (obj == bellCompleteObjects[0]) 
            {
                obj.GetComponent<RitualWall>().Rise(); 
            }
            obj.SetActive(true);
        }

        oldSky.SetActive(false);

        CheckComplete();
    }

    public void ShotBell()
    {
        bellsShot++;
        shotBell = true;
        bellTimer = timeBetweenBells;
    }

    public void ResetBells()
    {
        shotBell = false;
        bellsShot = 0;

        foreach (GameObject bell in bells)
        {
            bell.SetActive(true);
        }
    }

    private void Update()
    {
        if (shotBell)
        {
            bellTimer -= Time.deltaTime;
            if(bellsShot >= bellsToShoot)
            {
                CompletedBells();
            }
            if(bellTimer <= 0)
            {
                ResetBells();
            }
        }
    }

    public void PickupKnife()
    {
        hasKnife = true;
    }

    public void RandomizeListInPlace<T>(List<T> list)
    {
        int n = list.Count;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1); // Use static rng for true randomness
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
