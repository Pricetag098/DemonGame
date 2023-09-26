using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAttachments : MonoBehaviour
{
    [Header("Attachment System")]
    [Header("Head")]
    public List<GameObject> Horns;
    public List<GameObject> Choker;

    [Header("Arms")]
    public List<GameObject> ForearmLeft;
    public List<GameObject> ForearmRight;
    public List<GameObject> WristLeft;
    public List<GameObject> WristRight;

    [Header("Legs")]
    public List<GameObject> LowerLeftLeg;
    public List<GameObject> LowerRightLeg;
    public List<GameObject> AnklesLeft;
    public List<GameObject> AnklesRight;
    private List<GameObject> activeAttachments;

    private void Awake()
    {
        activeAttachments = new List<GameObject>();
    }

    public void RandomAttachments()
    {
        activeAttachments.Add(SetActiveFromList(Horns));
        activeAttachments.Add(SetActiveFromList(Choker));
        activeAttachments.Add(SetActiveFromList(ForearmLeft));
        activeAttachments.Add(SetActiveFromList(ForearmRight));
        activeAttachments.Add(SetActiveFromList(WristLeft));
        activeAttachments.Add(SetActiveFromList(WristRight));
        activeAttachments.Add(SetActiveFromList(LowerLeftLeg));
        activeAttachments.Add(SetActiveFromList(LowerRightLeg));
        activeAttachments.Add(SetActiveFromList(AnklesLeft));
        activeAttachments.Add(SetActiveFromList(AnklesRight));
    }

    private GameObject SetActiveFromList(List<GameObject> list)
    {
        int ran = Random.Range(0, list.Count);
        GameObject go = list[ran];
        go.SetActive(true);
        return go;
    }

    public void ResetAllAttachments()
    {
        foreach(GameObject g in activeAttachments)
        {
            g.SetActive(false);
        }
    }
}
