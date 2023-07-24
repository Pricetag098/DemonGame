using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerID : SoundPlayer
{
    public enum SoundSourceLocation
    {
        shoot,
        reload,
        empty
    }

    public SoundSourceLocation soundLoc;
}
