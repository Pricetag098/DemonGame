using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSceneLoader : MonoBehaviour
{
    private SubSceneReferences refs = SubSceneReferences.Instance;

    // get current area from detectarea
    private Areas CurrentArea { get { return DetectArea.CurrentArea; } }

    // break each area up into smaller chunks and load based off player position
    // grab the current area and where the player is facing to load / unload objects as the player moves around

    private void Awake()
    {
        
    }

    public void Start()
    {
        
    }

    public void Update()
    {
        
    }
}
