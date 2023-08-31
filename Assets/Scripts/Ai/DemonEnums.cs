using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonInfo
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
    public enum SpawnType
    {
        Null,
        Basic,
        Special,
        Boss
    }
}

