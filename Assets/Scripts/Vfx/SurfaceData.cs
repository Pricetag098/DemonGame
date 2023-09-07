using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SurfaceData")]
public class SurfaceData : ScriptableObject
{
    public Optional<VfxSpawnRequest> hitFx;
    public Optional<VfxSpawnRequest> stepFx;
}
