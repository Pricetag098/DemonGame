using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteMatch : MonoBehaviour
{
    private SpriteRenderer sprite;
    public Image image;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        
    }

    private void LateUpdate()
    {
        image.sprite = sprite.sprite;
    }
}
