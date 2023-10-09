using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyTrial : MonoBehaviour
{
    [SerializeField] List<GameObject> holyTrialPieces;

    int obeliskCounter = 0;

    public void ObeliskInteract()
    {
        holyTrialPieces[obeliskCounter].SetActive(true);

        if(obeliskCounter == 3)
        {
            FindObjectOfType<NarrativeInventory>().AllObelisks();
        }

        obeliskCounter++;    
    }
}
