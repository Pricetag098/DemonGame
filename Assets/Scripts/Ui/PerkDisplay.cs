using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PerkDisplay : MonoBehaviour
{
    [SerializeField] PerkManager perkManager;
    Image[] images;
    // Start is called before the first frame update
    void Awake()
    {
        images = GetComponentsInChildren<Image>();
        perkManager = FindObjectOfType<PerkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < images.Length; i++)
		{
            if(perkManager.perkList.Count > i)
			{
                images[i].sprite = perkManager.perkList[i].icon;
                images[i].gameObject.SetActive(true);
            }
			else
			{
                images[i].gameObject.SetActive(false);
			}
		}
    }
}
