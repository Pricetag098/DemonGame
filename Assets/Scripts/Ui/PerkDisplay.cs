using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PerkDisplay : MonoBehaviour
{
    [SerializeField] PerkManager perkManager;
    TextMeshProUGUI[] perkText;

    // Start is called before the first frame update
    void Awake()
    {
        perkText = GetComponentsInChildren<TextMeshProUGUI>();
        perkManager = FindObjectOfType<PerkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < perkText.Length; i++)
		{
            if(perkManager.perkList.Count > i)
			{
                perkText[i].text = perkManager.perkList[i].symbolText;
                perkText[i].font = perkManager.perkList[i].perkFont;
                perkText[i].gameObject.SetActive(true);
            }
			else
			{
                perkText[i].gameObject.SetActive(false);
			}
		}
    }
}
