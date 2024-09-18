using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EasterEggTracker : MonoBehaviour
{
    [Header("Bell Step")]
    [SerializeField] float timeBetweenBells;
    [SerializeField] int bellsToShoot;
    [SerializeField] List<GameObject> bells;
    [SerializeField] List<GameObject> bellCompleteObjects;

    [SerializeField] bool bellsDone;
    [SerializeField] int bellsShot;
    float bellTimer;
    bool shotBell;

    [Header("Knife Step")]
    [SerializeField] GameObject shovel;

    [SerializeField] bool hasKnife;

    [Header("Painting Step")]
    [SerializeField] List<GameObject> paintings;

    [SerializeField] int paintingInteracted;

    [SerializeField] bool paintingsDone;

    [Header("Skull Step")]
    [SerializeField] GameObject skullInteractable;


    [SerializeField] bool hasSkull;

    private static System.Random rng = new System.Random();

    private void Start()
    {
        RandomizeListInPlace(paintings);
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

    public void ComepletedPaintingPuzzle()
    {
        paintingsDone = true;
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

    public void PickupSkull()
    {
        hasSkull = true;
    }

    public void CompletedBells()
    {
        bellsDone = true;
        shotBell = false;

        foreach(GameObject bell in bells)
        {
            bell.SetActive(false);
        }

        foreach (GameObject obj in bellCompleteObjects)
        {
            obj.SetActive(true);
        }

        //Change Skybox
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
