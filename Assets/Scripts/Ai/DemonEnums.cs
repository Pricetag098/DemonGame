using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonCum
{
    [Serializable]
    public enum BuffType
    {
        Damage,
        Health,
        MoveSpeed,
        AttackSpeed
    }
    [Serializable]
    public enum DemonID
    {
        Null,
        Base,
        Summoner,
        Stalker,
        Chaos,
        Cultist
    }
}

