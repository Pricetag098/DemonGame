using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGlyph : MonoBehaviour
{
    public List<GameObject> glyphs;

    private void Start()
    {
        int ran = Random.Range(0, glyphs.Count);
        glyphs[ran].SetActive(true);
    }
}
