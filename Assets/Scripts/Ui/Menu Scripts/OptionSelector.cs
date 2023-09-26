using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionSelector : MonoBehaviour
{
    public int index;

    public TextMeshProUGUI text;

    public List<string> options= new List<string>();

    void Update()
    {
        if (index > options.Count - 1) { index = 0; }
        if (index < 0) { index = options.Count - 1; }
        text.text = options[index];
    }

    public void ButtonClick(int value)
    {
        index += value;
    }


}
