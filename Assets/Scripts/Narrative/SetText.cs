using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetText : MonoBehaviour
{
    [SerializeField] private NarrativeInventory narrativeInv;
    [SerializeField] private int order;
    [SerializeField] private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text.text = narrativeInv.paintings[order].GetComponent<Painting>().text;
    }

}
