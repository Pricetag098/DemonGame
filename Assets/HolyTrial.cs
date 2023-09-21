using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyTrial : MonoBehaviour
{
    [SerializeField] List<GameObject> holyTrialPieces;
    [SerializeField] GameObject finalPiece;
    [SerializeField] GameObject ritual;

    int obeliskCounter = 0;

    public void ObeliskInteract()
    {
        holyTrialPieces[obeliskCounter].SetActive(true);

        if(obeliskCounter == 3)
        {
            finalPiece.SetActive(true);
            ritual.SetActive(true);
        }

        obeliskCounter++;    }
}
