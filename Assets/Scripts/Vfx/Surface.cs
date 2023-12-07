using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour
{
    public SurfaceData data;
	public bool Penetrable = false;

#if UNITY_EDITOR
    private void Awake()
    {
        if (data == null)
            Debug.LogError("Missing a surface data", this);
    }
#endif
}
